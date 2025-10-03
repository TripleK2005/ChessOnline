using Microsoft.AspNetCore.Mvc;

namespace ChessOnline.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
