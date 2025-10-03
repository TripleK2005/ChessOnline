using Microsoft.AspNetCore.Mvc;

namespace ChessOnline.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
