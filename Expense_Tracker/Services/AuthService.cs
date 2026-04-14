using Expense_Tracker.Data;
using Expense_Tracker.DTO;
using ExpenseTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Expense_Tracker.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(AppDbContext context, IConfiguration configuration, PasswordHasher<User> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }
        public string Register(RegisterDTO dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return "User already exists";

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };

            user.HashPassword = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return "Registered successfully";
        }

        public string Login(LoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                return "User not found";

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashPassword, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return "Invalid password";

            // Here you would typically generate a JWT token and return it
            return GenerateJwtToken(user);
        }

        // 🔐 JWT TOKEN GENERATION
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
