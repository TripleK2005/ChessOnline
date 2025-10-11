using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChessOnline.Services;

[Authorize] // Yêu cầu người dùng phải đăng nhập để vào sảnh
public class GameController : Controller
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    public IActionResult Index() => View();

    public IActionResult Create()
    {
        var game = _gameService.CreateGame(out string roomCode);
        return RedirectToAction("Play", new { id = roomCode });
    }

    [HttpPost]
    public IActionResult Join(string roomCode)
    {
        var game = _gameService.GetGameByRoomCode(roomCode);
        if (game == null)
        {
            TempData["ErrorMessage"] = "Mã phòng không hợp lệ hoặc không tồn tại.";
            return RedirectToAction("Index");
        }

        if (game.WhitePlayer.HasValue && game.BlackPlayer.HasValue)
        {
            TempData["ErrorMessage"] = "Phòng đã đủ 2 người chơi.";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Play", new { id = roomCode });
    }

    [HttpGet("Game/Play/{id}")]
    public IActionResult Play(string id)
    {
        var game = _gameService.GetGameByRoomCode(id);
        if (game == null)
        {
            TempData["ErrorMessage"] = "Phòng không tồn tại.";
            return RedirectToAction("Index");
        }

        ViewBag.RoomCode = game.RoomCode;
        ViewBag.GameId = game.GameId; // Truyền Guid vào View
        return View();
    }
}