using Microsoft.AspNetCore.Mvc;

namespace ChessOnline.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
