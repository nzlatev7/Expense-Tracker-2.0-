namespace Expense_Tracker_2._0.Models.Request
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
    }
}
