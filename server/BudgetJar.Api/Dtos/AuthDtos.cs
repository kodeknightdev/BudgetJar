using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetJar.Api.Dtos
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
