﻿using Expense_Tracker_2._0.Models.DB;

namespace Expense_Tracker_2._0.Models.Response
{
    public class UserGetAllResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
