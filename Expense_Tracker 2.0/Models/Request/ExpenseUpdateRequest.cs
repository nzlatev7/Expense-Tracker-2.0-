﻿namespace Expense_Tracker_2._0.Models.Request
{
    public class ExpenseUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public string Date { get; set; }

        public decimal Amount { get; set; }
    }
}
