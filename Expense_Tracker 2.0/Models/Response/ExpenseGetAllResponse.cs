namespace Expense_Tracker_2._0.Models.Response
{
    public class ExpenseGetAllResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
}
