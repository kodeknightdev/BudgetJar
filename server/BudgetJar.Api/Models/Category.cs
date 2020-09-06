using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetJar.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public TransactionType Type { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [MaxLength(7)]
        public string Color { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
