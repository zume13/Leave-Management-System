
namespace LeaveManagement.Application.Dto.Response.Employee
{
    public sealed record RefreshTokenDto(
        string Accesstoken,
        DateTime AccessTokenExpiration,
        string RefreshToken,
        DateTime RefreshTokenExpiration);
}
