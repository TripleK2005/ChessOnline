ChessOnline - ASP.NET Core Razor Pages Project
==============================================

Mô tả:
-------
ChessOnline là một nền tảng chơi cờ vua trực tuyến, xây dựng bằng ASP.NET Core Razor Pages và Entity Framework Core.

Tính năng:
----------
- Đăng ký, đăng nhập người dùng (vai trò: Player/Admin)
- Tạo, tham gia và chơi ván cờ trực tuyến
- Theo dõi nước đi, lịch sử ván cờ
- Chat trong từng ván cờ
- Báo cáo người chơi vi phạm
- Quản trị viên (dự kiến)

Công nghệ sử dụng:
------------------
- ASP.NET Core 8.0 (Razor Pages)
- Entity Framework Core (SQL Server)
- SignalR (realtime)
- Bootstrap, jQuery Validation

Hướng dẫn chạy dự án:
---------------------
1. Cài đặt .NET 8 SDK và SQL Server Express (hoặc tương thích)
2. Clone project về máy:
   git clone https://github.com/TripleK2005/ChessOnline.git
3.Cấu hình kết nối và cài package

Cấu hình chuỗi kết nối trong appsettings.json

Cài các package:

-dotnet add package Microsoft.EntityFrameworkCore

-dotnet add package Microsoft.EntityFrameworkCore.SqlServer

-dotnet add package Microsoft.EntityFrameworkCore.Tools

-dotnet add package Microsoft.AspNetCore.SignalR

-dotnet add package Microsoft.AspNetCore.SignalR.Client


4. Tạo database và migration:
   xóa folder migration , chạy
   dotnet ef migrations add InitialCreate
   dotnet ef database update
5. Chạy ứng dụng:
   dotnet run
   hoặc F5 trong Visual Studio

Cấu trúc dự án:
<img width="457" height="652" alt="image" src="https://github.com/user-attachments/assets/697d79c1-cd78-481c-8795-d00ccc47487b" />


| Tính năng                    | Trạng thái   |
| ---------------------------- | ------------ |
| Thiết kế database & models   | ✅ Hoàn thành |
| Đăng ký/đăng nhập người dùng | ⬜ Chưa       |
| Logic tạo/tham gia ván cờ    | ⬜ Chưa       |
| Theo dõi nước đi             | ⬜ Chưa       |
| Chat trong ván cờ            | ⬜ Chưa       |
| Báo cáo người chơi           | ⬜ Chưa       |
| Trang quản trị Admin         | ⬜ Chưa       |
| Tính toán kết quả ván cờ     | ⬜ Chưa       |
| Hoàn thiện UI, responsive    | ⬜ Chưa       |
| Unit test                    | ⬜ Chưa       |


Đóng góp:
---------
- Fork repo, tạo branch mới, commit và tạo Pull Request

Bản quyền:
----------
MIT License

---

**Tiến độ hiện tại:**  
- Đã hoàn thành setup migration, kết nối database.
- Còn lại: rất nhiều ;))


