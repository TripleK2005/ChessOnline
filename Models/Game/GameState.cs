// using System.Collections.Generic;
// using System.Text.Json;

namespace ChessOnline.Models.Games;

public class GameState
{
    public Guid GameId { get; set; } = Guid.NewGuid();
    public string RoomCode { get; set; } // Mã phòng 6 số

    // Lưu thông tin người chơi
    public (int UserId, string ConnectionId)? WhitePlayer { get; set; }
    public (int UserId, string ConnectionId)? BlackPlayer { get; set; }
    public string Status { get; set; } = "Waiting"; // Waiting, Ongoing, Finished

    public string CurrentFen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    public string CurrentTurn { get; set; } = "w";

    public Stack<string> UndoStack { get; set; } = new();
    public Stack<string> RedoStack { get; set; } = new();
    public List<string> MoveHistory { get; set; } = new();

    public void Init()
    {
        CurrentFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        UndoStack.Clear();
        RedoStack.Clear();
        MoveHistory.Clear();
        UndoStack.Push(CurrentFen);
    }

    public bool IsPlayerTurn(int userId)
    {
        if (CurrentTurn == "w") return userId == WhitePlayer?.UserId;
        if (CurrentTurn == "b") return userId == BlackPlayer?.UserId;
        return false;
    }


}