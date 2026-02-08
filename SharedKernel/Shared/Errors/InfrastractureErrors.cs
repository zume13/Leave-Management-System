using SharedKernel.Shared.Result;

namespace SharedKernel.Shared.Errors
{
    public static class InfrastractureErrors
    {
        public static class General 
        { 
            public static readonly Error InternalError = Error.Failure("General.InternalError", "An internal error occurred");
        }

        public static class TokenService
        {
            public static readonly Error InvalidRefreshToken = Error.Problem("Token.InvalidRefresh", "The refresh token is invalid or expired");
            public static readonly Error TokenGenerationFailed = Error.Failure("Token.GenerationFailed", "Failed to generate token due to internal error");
            public static readonly Error InvalidInput = Error.Validation("Token.InvalidInput", "The provided input is invalid"); 
            public static readonly Error CredentialsError = Error.Failure("Creds.Error", "Error occurred while processing credentials");
            public static readonly Error TokenReused = Error.Failure("Token.Reused", "The refresh token has been reused");
        }
        public static class User 
        {
            public static readonly Error UserEmailExists = Error.Problem("User.Exists", "A user with the given email already exists");
            public static readonly Error FailedRegistry = Error.Failure("Register.Failed", "User registration failed due to internal error");
            public static readonly Error InvalidCredentials = Error.Problem("User.InvalidCredentials", "Invalid email or password");
            public static readonly Error UserNotFound = Error.NotFound("User.NotFound", "User not found");
        }

        public static class Email
        {
            public static readonly Error FailedToSendVerificationEmail = Error.Failure("Email.VerificationFailed", "Failed to send verification email");
            public static readonly Error EmailNotVerified = Error.Problem("Email.NotVerified", "Email address has not been verified");
        }
    }
}
