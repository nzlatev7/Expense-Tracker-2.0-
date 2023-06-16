﻿using Expense_Tracker_2._0.Models.DB;

namespace Expense_Tracker_2._0.Models.Request
{
    public class AdminUpdateRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
