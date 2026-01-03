
namespace SharedKernel.Shared
{
    public static class InfrastractureErrors
    {
        public static class TokenService
        {
            public static readonly Error InvalidRefreshToken = new("Token.InvalidRefresh", "The refresh token is invalid or expired");
        }
    }
}
