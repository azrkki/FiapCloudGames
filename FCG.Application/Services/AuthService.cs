using FCG.Application.DTOs;
using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using FCG.Core.Interfaces;
using FCG.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FCG.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpirationMinutes;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _jwtSecret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
            _jwtIssuer = _configuration["Jwt:Issuer"] ?? "FCG.Api";
            _jwtAudience = _configuration["Jwt:Audience"] ?? "FCG.Client";
            _jwtExpirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
            
            _logger.LogInformation("AuthService initialized with JWT settings - Issuer: {Issuer}, Audience: {Audience}, ExpirationMinutes: {ExpirationMinutes}", _jwtIssuer, _jwtAudience, _jwtExpirationMinutes);
        }

        public async Task<AuthResultDTO> LoginAsync(string email, string password)
        {
            _logger.LogInformation("LoginAsync called for email: {Email}", email);
            
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    _logger.LogWarning("LoginAsync failed: Email or password is empty");
                    return new AuthResultDTO
                    {
                        Success = false,
                        Message = "Email and password are required"
                    };
                }

                // Remove hardcoded system user - use database instead

                // Get user by email for normal users
                _logger.LogDebug("Attempting to retrieve user by email: {Email}", email);
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("LoginAsync failed: User not found for email: {Email}", email);
                    return new AuthResultDTO
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                // Verify password using the Password value object's verification method
                if (!user.Password.VerifyPassword(password))
                {
                    _logger.LogWarning("LoginAsync failed: Invalid password for user: {Email}", email);
                    return new AuthResultDTO
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);
                var expiresAt = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes);

                _logger.LogInformation("LoginAsync successful for user: {Email}", email);
                return new AuthResultDTO
                {
                    Success = true,
                    Token = token,
                    Message = "Login successful",
                    User = new UserAuthResponseDTO
                    {
                        Name = user.Name,
                        Email = user.Email
                    },
                    ExpiresAt = expiresAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in LoginAsync for email: {Email}", email);
                return new AuthResultDTO
                {
                    Success = false,
                    Message = "An error occurred during login"
                };
            }
        }

        public bool ValidateToken(string token)
        {
            _logger.LogDebug("ValidateToken called");
            
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                _logger.LogDebug("Token validation successful");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return false;
            }
        }

        public UserTokenInfoDTO GetUserFromToken(string token)
        {
            _logger.LogDebug("GetUserFromToken called");
            
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                
                // First try to read the token without validation to see the claims
                var jwtToken = tokenHandler.ReadJwtToken(token);
                
                // Log all claims for debugging
                _logger.LogDebug("Available claims in token: {Claims}", string.Join(", ", jwtToken.Claims.Select(c => $"{c.Type}={c.Value}")));

                var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "id");
                var nameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "name");
                var emailClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "email");
                var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role) ?? 
                               jwtToken.Claims.FirstOrDefault(x => x.Type == "role");
                
                _logger.LogDebug("Claim extraction results - UserId: {UserIdFound}, Name: {NameFound}, Email: {EmailFound}, Role: {RoleFound}", 
                    userIdClaim != null, nameClaim != null, emailClaim != null, roleClaim != null);
                
                if (userIdClaim == null || nameClaim == null || emailClaim == null)
                {
                    _logger.LogWarning("Missing required claims in token");
                    return null;
                }
                
                var userId = int.Parse(userIdClaim.Value);
                var name = nameClaim.Value;
                var email = emailClaim.Value;
                var role = roleClaim?.Value ?? "Common";
                
                _logger.LogDebug("Successfully extracted user info from token for user: {Email} (ID: {UserId})", email, userId);

                return new UserTokenInfoDTO
                {
                    Name = name,
                    Email = email
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting user info from token. Token: {Token}", token);
                return null;
            }
        }

        private string GenerateJwtToken(FCG.Core.Entity.User user)
        {
            _logger.LogDebug("Generating JWT token for user: {Email} (ID: {UserId})", user.Email, user.Id);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "Common")
            };
            
            _logger.LogDebug("JWT claims created for user: {Email} with role: {Role}. ClaimTypes.Role value: {ClaimTypeRole}", user.Email, user.Role?.Name ?? "Common", ClaimTypes.Role);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}