using ChessOnline.Models.Admin;
using ChessOnline.Models.Enums;
using ChessOnline.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessOnline.Controllers
{
    // ✅ Bật authorize cho admin
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // ✅ GET: /Admin/ManageUsers
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _adminService.GetAllUsersAsync();

            // Map DTO → ViewModel
            var viewModels = users.Select(u => new ManageUsersViewModel
            {
                UserID = u.UserID,
                Username = u.Username,
                Email = u.Email,
                Avatar = u.Avatar,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                Status = u.Status // ✅ cần có trong ViewModel
            }).ToList();

            ViewData["Title"] = "Quản lý người dùng";
            return View(viewModels);
        }

        // ✅ GET: /Admin/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _adminService.GetUserByIdAsync(id);
            if (!result.Success || result.Data == null)
            {
                TempData["Message"] = "Không tìm thấy người dùng.";
                return RedirectToAction("ManageUsers");
            }

            var user = result.Data;
            var viewModel = new ManageUsersViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Avatar = user.Avatar,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                Status = user.Status
            };

            return View(viewModel);
        }


        // ✅ Tạo mới user (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Tạo mới user (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new ChessOnline.DTOs.AdminDtos.ManageUserDto
            {
                Username = model.Username,
                Email = model.Email,
                Role = model.Role,
                Avatar = model.Avatar
            };

            // fake password mặc định (có thể random)
            var result = await _adminService.CreateUserAsync(dto, model.Password);
            TempData["Message"] = result.Success ? "Tạo người dùng thành công!" : result.Message;

            return RedirectToAction("ManageUsers");
        }

        // ✅ Cập nhật (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _adminService.GetUserByIdAsync(id);
            if (!result.Success) return NotFound();

            var user = result.Data;
            var model = new EditUserViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

            return View(model);
        }

        // ✅ Cập nhật (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _adminService.UpdateUserRoleAsync(model.UserID, model.Role);
            TempData["Message"] = result.Success ? "Cập nhật thông tin thành công!" : result.Message;

            return RedirectToAction("ManageUsers");
        }

        // ✅ Xóa người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _adminService.DeleteUserAsync(id);
            TempData["Message"] = result.Success ? "Đã xóa tài khoản!" : result.Message;

            return RedirectToAction("ManageUsers");
        }

        // ✅ Trang chính admin (Dashboard)
        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }

        // ✅ POST: /Admin/BanUser/5
        [HttpPost]
        [ValidateAntiForgeryToken] // 🔒 thêm bảo vệ form
        public async Task<IActionResult> BanUser(int id)
        {
            var result = await _adminService.BanUserAsync(id);
            TempData["Message"] = result.Success
                ? "✅ Đã khóa tài khoản thành công!"
                : $"❌ {result.Message}";

            return RedirectToAction("ManageUsers");
        }

        // ✅ POST: /Admin/UnbanUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnbanUser(int id)
        {
            var result = await _adminService.UnbanUserAsync(id);
            TempData["Message"] = result.Success
                ? "✅ Đã mở khóa tài khoản thành công!"
                : $"❌ {result.Message}";

            return RedirectToAction("ManageUsers");
        }
    }
}
