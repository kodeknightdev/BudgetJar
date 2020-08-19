using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetJar.Api.Dtos
{
    public class SaveTransactionRequest
    {
        [Range(typeof(decimal), "0.01", "1000000")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}
