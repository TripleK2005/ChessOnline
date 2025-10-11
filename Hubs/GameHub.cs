using ChessOnline.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;

[Authorize]
public class GameHub : Hub
{
    private readonly IGameService _gameService;

    public GameHub(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task JoinGame(string gameIdStr)
    {
        if (!Guid.TryParse(gameIdStr, out Guid gameId)) return;

        var game = _gameService.GetGameById(gameId);
        if (game == null) return;

        var userId = int.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var connectionId = Context.ConnectionId;

        // Thêm người chơi vào nhóm SignalR
        await Groups.AddToGroupAsync(connectionId, gameId.ToString());

        // Gán quân cờ cho người chơi
        bool isPlayer1 = false;
        if (!game.WhitePlayer.HasValue)
        {
            game.WhitePlayer = (userId, connectionId);
            isPlayer1 = true;
        }
        else if (!game.BlackPlayer.HasValue && game.WhitePlayer.Value.UserId != userId)
        {
            game.BlackPlayer = (userId, connectionId);
        }

        // Nếu đủ 2 người chơi, bắt đầu game
        if (game.WhitePlayer.HasValue && game.BlackPlayer.HasValue)
        {
            game.Status = "Ongoing";
            // Gửi sự kiện bắt đầu cho cả 2
            await Clients.Client(game.WhitePlayer.Value.ConnectionId).SendAsync("GameStarted", "w", game.CurrentFen);
            await Clients.Client(game.BlackPlayer.Value.ConnectionId).SendAsync("GameStarted", "b", game.CurrentFen);
        }
    }

    public async Task SendMove(string gameIdStr, object moveData)
    {
        if (!Guid.TryParse(gameIdStr, out Guid gameId)) return;
        var game = _gameService.GetGameById(gameId);
        if (game == null || game.Status != "Ongoing") return;

        var userId = int.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));

        // Kiểm tra xem có đúng lượt của người chơi này không
        if (!game.IsPlayerTurn(userId))
        {
            // Có thể gửi lại lỗi cho client nếu cần
            return;
        }

        // Bóc tách dữ liệu từ client
        var move = (JsonElement)moveData;
        string from = move.GetProperty("from").GetString();
        string to = move.GetProperty("to").GetString();
        string fen = move.GetProperty("after").GetString(); // Lấy FEN sau nước đi từ client

        _gameService.ApplyMove(gameId, from, to, fen);

        // Gửi nước đi cho tất cả người chơi trong phòng (bao gồm cả người vừa đi)
        await Clients.Group(gameId.ToString()).SendAsync("ReceiveMove", moveData);
    }

    // (Các hub method khác như UndoMove có thể thêm vào sau)
}