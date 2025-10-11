using ChessOnline.Models.Games;
using System;

namespace ChessOnline.Services;

public interface IGameService
{
    /// <summary>
    /// Tạo một ván cờ mới và trả về mã phòng 6 số qua tham số out.
    /// </summary>
    /// <param name="roomCode">Mã phòng 6 số được tạo ra.</param>
    /// <returns>Trạng thái của game mới.</returns>
    GameState CreateGame(out string roomCode);

    /// <summary>
    /// Tìm một ván cờ bằng mã phòng (string 6 số).
    /// </summary>
    /// <param name="roomCode">Mã phòng của người dùng.</param>
    /// <returns>Trạng thái game hoặc null nếu không tìm thấy.</returns>
    GameState GetGameByRoomCode(string roomCode);

    /// <summary>
    /// Tìm một ván cờ bằng GameId (Guid) nội bộ.
    /// </summary>
    /// <param name="gameId">Guid của game.</param>
    /// <returns>Trạng thái game hoặc null nếu không tìm thấy.</returns>
    GameState GetGameById(Guid gameId);

    /// <summary>
    /// Áp dụng một nước đi vào trạng thái game.
    /// </summary>
    /// <returns>Trạng thái game sau khi đã đi.</returns>
    GameState ApplyMove(Guid gameId, string from, string to, string newFen);

    /// <summary>
    /// Hoàn tác nước đi cuối cùng.
    /// </summary>
    /// <returns>Trạng thái game sau khi hoàn tác.</returns>
    GameState Undo(Guid gameId);

    /// <summary>
    /// Làm lại nước đi đã hoàn tác.
    /// </summary>
    /// <returns>Trạng thái game sau khi làm lại.</returns>
    GameState Redo(Guid gameId);
}