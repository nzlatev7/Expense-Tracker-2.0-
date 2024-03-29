﻿using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker_2._0.Models.DB
{
    public enum Role
    { 
        Admin,
        Customer,
    }

    public class User
    {
        //[Required] - not null
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] // this is like VarChar(50)
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public Role Role { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        //one to many
        public ICollection<Expense> Expenses { get; set; }
    }
}
