using Microsoft.AspNetCore.Mvc;

namespace ChessOnline.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
