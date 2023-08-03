namespace Expense_Tracker_2._0.Constants
{
    public static class ResponseMessages
    {
        public const string UserNotUnique = "Username is not unique";
        public const string UserNameLength = "Username Length";
        public const string PasswordLength = "Password Length";
        public const string PasswordInvalid = "Invalid password";

        public const string EmailExists = "There is already a user registered with this email address.";
        public const string EmailInvalid = "Invalid Email";
        public const string EmailVerified = "Verified! You have successfully verified your account.";

        public const string InvalidVerificationCode = "Invalid verification code. Please enter the correct code or request a new one.";
        public const string VerificationCodeExpired = "Verification code has expired. Please request a new one.";

        public const string WrongUsernameOrPass = "Wrong username or password.";
        public const string EmailVerificationRequired = "Please verify your email address";

        public const string PageNumInvalid = "Page number must be a positive integer.";

        //expense
        public const string NameLength = "Name Lenght";
        public const string AmountInvalid = "Invalid Amount";
    }
}
