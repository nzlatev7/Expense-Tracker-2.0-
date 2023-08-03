namespace Expense_Tracker_2._0.Constants
{
    public static class AppConstants
    {
        //Username must be unique and between 4 and 25 symbols.
        public const int UsernameMinLength = 4;
        public const int UsernameMaxLength = 25;

        //Password must be unique and between 8 and 25 symbols.
        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 25;

        //Email must be valid email and unique in the system.
        public const string EmailRegex = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

        //Expense

        //Validation for Name Lenght, (name -> 1 - 30)
        public const int NameMinLength = 1;
        public const int NameMaxLength = 30;

        //Validation for expense amount, (amount -> 0 - 10000)
        public const int AmountMin = 0;
        public const int AmountMax = 10000;

        //admin password
        public const string AdminPassword = "admin";


        //email sending
        public const string EmailSubjectVerification = "Verify your Email Address!";
        public const string EmailBodyVerification = "This is the token: {token.Value} which expire on {token.ExpirationDate}";

        public const string PasswordSubjectVerification = "Reset Password";
        public const string PasswordBodyVerification = "This is the token: {token.Value} which expire on {token.ExpirationDate}";
    }
}
