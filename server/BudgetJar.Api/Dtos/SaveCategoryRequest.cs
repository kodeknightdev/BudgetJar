using System.ComponentModel.DataAnnotations;
using BudgetJar.Api.Models;

namespace BudgetJar.Api.Dtos
{
    public class SaveCategoryRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [EnumDataType(typeof(TransactionType))]
        public TransactionType Type { get; set; }
    }
}
