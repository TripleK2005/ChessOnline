using ChessOnline.Models.Account;
using ChessOnline.Services;
using Microsoft.AspNetCore.Mvc;
using ChessOnline.DTOs.AccountDtos;
using ChessOnline.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ChessOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Gọi service đăng ký (tùy bạn implement)
            var result = await _userService.RegisterAsync(new UserRegisterDto
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            });

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            // Nếu đăng ký thành công => chuyển qua trang Login
            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ✅ Tạo DTO đúng kiểu
            var loginDto = new UserLoginDto
            {
                Username = model.Username,
                Password = model.Password
            };

            // ✅ Gọi service đăng nhập với DTO
            var result = await _userService.LoginAsync(loginDto);

            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError("", result.Message ?? "Sai tài khoản hoặc mật khẩu");
                return View(model);
            }

            var user = result.Data;

            // ✅ Tạo claims để đăng nhập cookie
            var claims = new List<Claim>
    {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Role,user.Role ?? "Player")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3)
            };

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);


            return RedirectToAction("Profile");
        }


        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Nếu chưa đăng nhập thì đưa về trang Login
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            // Lấy userId từ claim (được lưu khi login)
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return RedirectToAction("Login");

            int userId = int.Parse(userIdClaim);

            // Gọi service lấy thông tin người dùng từ DB
            var result = await _userService.GetProfileAsync(userId);

            if (!result.Success || result.Data == null)
            {
                ViewBag.Message = "Không tìm thấy thông tin người dùng.";
                return View(null);
            }

            // Truyền dữ liệu sang view
            return View(result.Data);
        }

        // POST: /Account/UploadAvatar
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");
            if (avatar == null || avatar.Length == 0)
            {
                TempData["Message"] = "Vui lòng chọn tệp ảnh.";
                return RedirectToAction("Profile");
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // Gọi service để lưu ảnh đại diện
            var result = await _userService.UploadAvatarAsync(userId, avatar);
            if (!result.Success)
            {
                TempData["Message"] = result.Message;
                return RedirectToAction("Profile");
            }
            TempData["Message"] = "Cập nhật ảnh đại diện thành công!";
            return RedirectToAction("Profile");
        }

        // GET: /Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new ChangePasswordViewModel { UserId = userId };
            return View(model);
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.ChangePasswordAsync(new ChangePasswordDto
            {
                UserId = model.UserId,
                OldPassword = model.OldPassword,
                NewPassword = model.NewPassword
            });

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            ViewBag.Message = "Password changed successfully!";
            return View(model);
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
