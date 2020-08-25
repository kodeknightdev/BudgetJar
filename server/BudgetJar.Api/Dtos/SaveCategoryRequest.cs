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

        [RegularExpression("^#[0-9a-fA-F]{6}$", ErrorMessage = "Color must be a hex value like #4e79a7.")]
        public string Color { get; set; }
    }
}
