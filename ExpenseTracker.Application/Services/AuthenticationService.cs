using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Security;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Application.Services;

public class AuthenticationService : IAuthenticationService
{
  private readonly IRepositoryManager _repository;
  private readonly IPasswordHasher _passwordHasher;
  private readonly IJwtTokenService _jwtTokenService;

  public AuthenticationService(IRepositoryManager repository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
  {
    _repository = repository;
    _passwordHasher = passwordHasher;
    _jwtTokenService = jwtTokenService;
  }

  public async Task<string> RegisterAsync(RegisterRequestDto dto)
  {
    var existingUser = await _repository.Users.GetByLoginIdAsync(dto.LoginId);
    if (existingUser != null)
      throw new ConflictException("User with this LoginId already exists.");

    var user = new User
    {
      LoginId = dto.LoginId,
      UserName = dto.UserName,
      PasswordHash = _passwordHasher.HashPassword(dto.Password),
      Email = dto.Email,
      IsActive = true,
      CreatedDate = DateTime.UtcNow
    };

    _repository.Users.Create(user);
    await _repository.SaveAsync();
    return "User registered successfully.";
  }

  public async Task<TokenResponseDto> LoginAsync(LoginRequestDto dto)
  {
    var user = await _repository.Users.GetByLoginIdAsync(dto.LoginId)
      ?? throw new UnauthorizedException("Invalid credentials.");

    if (!user.IsActive)
      throw new UnauthorizedException("User account is deactivated.");

    if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
      throw new UnauthorizedException("Invalid credentials.");

    var accessToken = _jwtTokenService.GenerateAccessToken(user);
    var refreshToken = _jwtTokenService.GenerateRefreshToken();
    var accessTokenExpiry = DateTime.UtcNow.AddMinutes(15);
    var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

    var refreshTokenEntity = new RefreshToken
    {
      UserId = user.UserId,
      Token = _jwtTokenService.HashToken(refreshToken),
      ExpiryDate = refreshTokenExpiry,
      CreatedDate = DateTime.UtcNow,
      CreatedByIp = "0.0.0.0",
      IsRevoked = false
    };

    _repository.RefreshTokens.Create(refreshTokenEntity);

    user.LastLoginDate = DateTime.UtcNow;
    _repository.Users.Update(user);
    await _repository.SaveAsync();

    return new TokenResponseDto
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      AccessTokenExpiry = accessTokenExpiry,
      RefreshTokenExpiry = refreshTokenExpiry,
      TokenType = "Bearer",
      ExpiresIn = 900
    };
  }

  public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress)
  {
    var hashedToken = _jwtTokenService.HashToken(refreshToken);
    var storedToken = await _repository.RefreshTokens.GetByIdAsync(t => t.Token == hashedToken, trackChanges: true)
      ?? throw new UnauthorizedException("Refresh token not found.");

    if (storedToken.IsRevoked)
    {
      await RevokeAllUserTokensAsync(storedToken.UserId, ipAddress);
      throw new UnauthorizedException("Token reuse detected. All tokens have been revoked for security.");
    }

    if (!storedToken.IsActive)
      throw new UnauthorizedException("Refresh token is expired.");

    var user = await _repository.Users.GetByIdAsync(u => u.UserId == storedToken.UserId)
      ?? throw new UnauthorizedException("User not found.");

    var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
    var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
    var accessTokenExpiry = DateTime.UtcNow.AddMinutes(15);
    var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

    storedToken.IsRevoked = true;
    storedToken.RevokedDate = DateTime.UtcNow;
    storedToken.ReplacedByToken = _jwtTokenService.HashToken(newRefreshToken);

    var newRefreshTokenEntity = new RefreshToken
    {
      UserId = user.UserId,
      Token = _jwtTokenService.HashToken(newRefreshToken),
      ExpiryDate = refreshTokenExpiry,
      CreatedDate = DateTime.UtcNow,
      CreatedByIp = ipAddress,
      IsRevoked = false
    };

    _repository.RefreshTokens.Create(newRefreshTokenEntity);
    await _repository.SaveAsync();

    return new TokenResponseDto
    {
      AccessToken = newAccessToken,
      RefreshToken = newRefreshToken,
      AccessTokenExpiry = accessTokenExpiry,
      RefreshTokenExpiry = refreshTokenExpiry,
      TokenType = "Bearer",
      ExpiresIn = 900
    };
  }

  public async Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress)
  {
    var hashedToken = _jwtTokenService.HashToken(refreshToken);
    var storedToken = await _repository.RefreshTokens.GetByIdAsync(t => t.Token == hashedToken, trackChanges: true);
    if (storedToken == null || !storedToken.IsActive)
      return false;

    storedToken.IsRevoked = true;
    storedToken.RevokedDate = DateTime.UtcNow;
    await _repository.SaveAsync();
    return true;
  }

  public async Task RemoveExpiredTokensAsync()
  {
    await _repository.RefreshTokens.RemoveExpiredTokensAsync();
    await _repository.SaveAsync();
  }

  public async Task RevokeAllUserTokensAsync(int userId, string ipAddress)
  {
    var activeTokens = await _repository.RefreshTokens.GetActiveTokensByUserIdAsync(userId);
    foreach (var token in activeTokens)
    {
      token.IsRevoked = true;
      token.RevokedDate = DateTime.UtcNow;
    }
    await _repository.SaveAsync();
  }

  public async Task<UserInfoDto> GetUserInfoAsync(int userId)
  {
    var user = await _repository.Users.GetByIdAsync(u => u.UserId == userId)
      ?? throw new NotFoundException(nameof(User), userId);

    return new UserInfoDto
    {
      UserId = user.UserId,
      LoginId = user.LoginId,
      UserName = user.UserName,
      Email = user.Email,
      LastLoginDate = user.LastLoginDate
    };
  }
}
