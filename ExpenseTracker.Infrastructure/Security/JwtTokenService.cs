using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExpenseTracker.Application.Interfaces.Security;
using ExpenseTracker.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Infrastructure.Security;

public class JwtTokenService : IJwtTokenService
{
  private readonly IConfiguration _configuration;

  public JwtTokenService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string GenerateAccessToken(User user)
  {
    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
      new Claim("LoginId", user.LoginId),
      new Claim(ClaimTypes.Name, user.UserName),
      new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
    };

    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
    var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    var tokenOptions = new JwtSecurityToken(
      issuer: _configuration["Jwt:Issuer"],
      audience: _configuration["Jwt:Audience"],
      claims: claims,
      notBefore: DateTime.UtcNow,
      expires: DateTime.UtcNow.AddMinutes(15),
      signingCredentials: signingCredentials
    );

    return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
  }

  public string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber)
      .Replace('+', '-')
      .Replace('/', '_')
      .TrimEnd('=');
  }

  public string HashToken(string token)
  {
    using var sha256 = SHA256.Create();
    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
    return Convert.ToBase64String(hashedBytes);
  }
}
