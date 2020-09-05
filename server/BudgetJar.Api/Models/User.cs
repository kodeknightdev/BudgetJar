using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetJar.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
