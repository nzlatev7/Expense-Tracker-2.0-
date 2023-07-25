namespace Expense_Tracker_2._0.Models.Request
{
    public class UserVerifyResetPasswordToken
    {
        public string TokenValue { get; set; }

        public string NewPassword { get; set; }
    }
}
