using System;
using BudgetJar.Api.Models;

namespace BudgetJar.Api.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public TransactionType Type { get; set; }
    }
}
