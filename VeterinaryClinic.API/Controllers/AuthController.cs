using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VeterinaryClinic.API.Configuration;
using VeterinaryClinic.API.Models.Auth;
using VeterinaryClinic.DataAccess.Context;
using VeterinaryClinic.Entities;
using BCrypt.Net;

namespace VeterinaryClinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly VeterinaryClinicDbContext _dbContext;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        VeterinaryClinicDbContext dbContext,
        IOptions<JwtSettings> jwtSettings)
    {
        _dbContext = dbContext;
        _jwtSettings = jwtSettings.Value;
    }

    // POST: /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // 1) Email normalise et
        var email = request.Email.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and pasword are required.");
        }

        // 2) Email zaten var mı?

        var exists = await _dbContext.Users
            .AnyAsync(u => u.Email == email);

        if (exists)
        {
            return BadRequest("A user with this email already exists.");
        }

        // 3) Rol Belirler (default: Customer "Müşteri")
        var role = UserRole.Customer;

        if (!string.IsNullOrWhiteSpace(request.Role)
            && request.Role.Trim().Equals("Manager", StringComparison.OrdinalIgnoreCase))
        {
            role = UserRole.Manager;
        }

        // 4) Şifreyi hashle

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 5) Yeni kullanıcı oluştur

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = passwordHash,
            Role = role
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Register sonrası direkt login olmuş gibi token dönme

       return Ok(CreateAuthResponse(user));

    }

    // POST: /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // Kullanıcı yok
            return Unauthorized("Invalid email or password.");
        }

        bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        // Şifre doğru mu?
        if (!verified)
        {
            return Unauthorized("Invalid email or password.");
        }


        var authResponse = CreateAuthResponse(user);

        return Ok(authResponse);
    }

    // Şifre hash/verify yardımcıları

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha.ComputeHash(bytes);

        // Hex string'e çevir
        return Convert.ToHexString(hashBytes);
    }
    

    private static bool VerifyPassword(string password, string storedHash)
    {
        var hashOfInput = HashPassword(password);
        return string.Equals(hashOfInput, storedHash, StringComparison.OrdinalIgnoreCase);
    }

    // JWT üretimi

    private AuthResponse CreateAuthResponse(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FullName),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        // 1) Token nesnesi
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        // 2) String’e çeviriyoruz
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // 3) Dışarıya döneceğimiz DTO
        return new AuthResponse
        {
            Token = tokenString,
            ExpiresAt = expires,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
