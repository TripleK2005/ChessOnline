using ChessOnline.Models.Games;
using System.Collections.Concurrent;

namespace ChessOnline.Services;

public class GameService : IGameService // Đảm bảo bạn có Interface này
{
    // _games: Lưu GameState bằng Guid
    private static readonly ConcurrentDictionary<Guid, GameState> _games = new();
    // _roomCodeToGameId: Ánh xạ mã phòng (string) tới GameId (Guid)
    private static readonly ConcurrentDictionary<string, Guid> _roomCodeToGameId = new();

    // Tạo game mới và trả về mã phòng
    public GameState CreateGame(out string roomCode)
    {
        var game = new GameState();
        game.Init();

        // Sinh mã phòng duy nhất
        roomCode = new Random().Next(100000, 999999).ToString();
        while (!_roomCodeToGameId.TryAdd(roomCode, game.GameId))
        {
            roomCode = new Random().Next(100000, 999999).ToString();
        }

        game.RoomCode = roomCode;
        _games.TryAdd(game.GameId, game);
        return game;
    }

    // Lấy GameState bằng mã phòng
    public GameState GetGameByRoomCode(string roomCode)
    {
        if (_roomCodeToGameId.TryGetValue(roomCode, out Guid gameId))
        {
            return GetGameById(gameId);
        }
        return null;
    }

    public GameState GetGameById(Guid gameId)
    {
        _games.TryGetValue(gameId, out var game);
        return game;
    }

    // Logic xử lý nước đi của bạn (giữ nguyên)
    public GameState ApplyMove(Guid gameId, string from, string to, string newFen)
    {
        var game = GetGameById(gameId);
        if (game == null) return null;

        game.UndoStack.Push(game.CurrentFen); // Lưu FEN cũ vào stack
        game.RedoStack.Clear();
        game.MoveHistory.Add($"{from}-{to}");
        game.CurrentFen = newFen;
        game.CurrentTurn = GuessTurnFromFen(newFen);
        return game;
    }

    // (Các phương thức khác như Undo, Redo, GuessTurnFromFen... giữ nguyên)
    public GameState Undo(Guid gameId)
    {
        var game = GetGameById(gameId);
        if (game == null || game.UndoStack.Count <= 1)
            return game;

        var top = game.UndoStack.Pop();
        game.RedoStack.Push(top);

        var prevFen = game.UndoStack.Peek();
        game.CurrentFen = prevFen;
        // Cập nhật lại lượt đi sau khi Undo
        game.CurrentTurn = GuessTurnFromFen(prevFen);

        if (game.MoveHistory.Count > 0)
            game.MoveHistory.RemoveAt(game.MoveHistory.Count - 1);

        return game;
    }

    public GameState Redo(Guid gameId)
    {
        var game = GetGameById(gameId);
        if (game == null || game.RedoStack.Count == 0)
            return game;

        var fen = game.RedoStack.Pop();
        game.UndoStack.Push(fen);
        game.CurrentFen = fen;
        // Cập nhật lại lượt đi sau khi Redo
        game.CurrentTurn = GuessTurnFromFen(fen);

        // (Lưu ý: Logic này chưa khôi phục MoveHistory, có thể cải thiện sau)
        return game;
    }
    private string GuessTurnFromFen(string fen)
    {
        var parts = fen.Split(' ');
        return parts.Length >= 2 ? parts[1] : "w";
    }
    // ...
}