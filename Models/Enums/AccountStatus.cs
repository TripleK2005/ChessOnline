namespace ChessOnline.Models.Enums
{
    public enum AccountStatus
    {
        Active = 0,             // hoạt động bình thường
        Banned = 1,             // bị cấm vĩnh viễn
        Suspended = 2,          // tạm khóa
        PendingVerification = 3 // chờ xác minh
    }
}