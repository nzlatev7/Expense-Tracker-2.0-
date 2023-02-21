using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker_2._0.Models.DB
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Date { get; set; }

        [Required]
        [MaxLength(50)]
        public decimal Amount { get; set; }


        //one to many relationship

        [ForeignKey(nameof(User))] //attribute  
        public int UserId { get; set; } //FK
        public User User { get; set; } //navigational property
    }
}
