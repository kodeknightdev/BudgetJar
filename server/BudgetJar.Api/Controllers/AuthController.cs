using System;
using System.Threading.Tasks;
using BudgetJar.Api.Data;
using BudgetJar.Api.Dtos;
using BudgetJar.Api.Models;
using BudgetJar.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetJar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BudgetContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly TokenService _tokens;

        public AuthController(BudgetContext db, IPasswordHasher<User> hasher, TokenService tokens)
        {
            _db = db;
            _hasher = hasher;
            _tokens = tokens;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            if (await _db.Users.AnyAsync(u => u.Email == email))
            {
                return Conflict(new { message = "An account with that email already exists." });
            }

            var user = new User { Email = email, CreatedAt = DateTime.UtcNow };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            _db.Categories.AddRange(DefaultCategories.For(user));
            await _db.SaveChangesAsync();

            return _tokens.CreateToken(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
            }

            return _tokens.CreateToken(user);
        }
    }
}
