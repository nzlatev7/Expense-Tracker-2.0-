using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker_2._0.Models.DB
{
    public class ValidationToken
    {
        [Key]
        public string Value { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int UserId { get; set; }

        public int AttemptsCount { get; set; }
    }
}
