using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetJar.Api.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
