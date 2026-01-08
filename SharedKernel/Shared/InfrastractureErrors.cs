
namespace SharedKernel.Shared
{
    public static class InfrastractureErrors
    {
        public static class General 
        { 
            public static readonly Error InternalError = new("General.InternalError", "An internal error occurred");
        }

        public static class TokenService
        {
            public static readonly Error InvalidRefreshToken = new("Token.InvalidRefresh", "The refresh token is invalid or expired");
            public static readonly Error TokenGenerationFailed = new("Token.GenerationFailed", "Failed to generate token due to internal error");
            public static readonly Error InvalidInput = new("Token.InvalidInput", "The provided input is invalid"); 
            public static readonly Error CredentialsError = new("Creds.Error", "Error occurred while processing credentials");
            public static readonly Error TokenReused = new("Token.Reused", "The refresh token has been reused");
        }
        public static class User 
        {
            public static readonly Error UserEmailExists = new("User.Exists", "A user with the given email already exists");
            public static readonly Error FailedRegistry = new("Register.Failed", "User registration failed due to internal error");
            public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid email or password");
            public static readonly Error UserNotFound = new("User.NotFound", "User not found");
        }

        public static class Email
        {
            public static readonly Error FailedToSendVerificationEmail = new("Email.VerificationFailed", "Failed to send verification email");
            public static readonly Error EmailNotVerified = new("Email.NotVerified", "Email address has not been verified");
        }
    }
}
