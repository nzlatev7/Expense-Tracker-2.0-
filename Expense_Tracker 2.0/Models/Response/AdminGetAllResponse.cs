using Expense_Tracker_2._0.Models.DB;

namespace Expense_Tracker_2._0.Models.Response
{
    public class AdminGetAllResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string? Email { get; set; }

        //in the future we will yield return this expenses
        //public List<Expense> Expenses { get; set; }
    }
}
