﻿using Expense_Tracker_2._0.Models.DB;

namespace Expense_Tracker_2._0.Models.Response
{
    public class UserGetInfoResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }

        //public List<Expense> Expenses { get; set; } - this will happend in the Expense functionality
    }
}