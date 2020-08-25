using BudgetJar.Api.Models;

namespace BudgetJar.Api.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TransactionType Type { get; set; }
        public string Color { get; set; }
    }
}
