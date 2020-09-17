using BudgetJar.Api.Models;

namespace BudgetJar.Api.Dtos
{
    public static class MappingExtensions
    {
        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
                Color = category.Color
            };
        }

        public static TransactionDto ToDto(this Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Name,
                Type = transaction.Category.Type
            };
        }
    }
}
