using FCG.Application.DTOs;
using FCG.Application.Services;
using FCG.Application.Interfaces;
using FCG.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>Authentication result with JWT token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            _logger.LogInformation("Login attempt initiated for user: {Email}", loginRequest?.Email ?? "Unknown");
            
            try
            {
                if (loginRequest == null)
                {
                    _logger.LogWarning("Login attempt failed: Login request is null");
                    return BadRequest(new { message = "Login request is required" });
                }

                if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    _logger.LogWarning("Login attempt failed: Missing email or password for user: {Email}", loginRequest.Email);
                    return BadRequest(new { message = "Email and password are required" });
                }

                // Check if user is already logged in
                var existingToken = Request.Cookies["jwt"] ?? Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "") ?? string.Empty;
                
                if (!string.IsNullOrEmpty(existingToken))
                {
                    _logger.LogInformation("Checking existing token for user: {Email}", loginRequest.Email);
                    // Validate the existing token
                    var isValidToken = _authService.ValidateToken(existingToken);
                    if (isValidToken)
                    {
                        _logger.LogWarning("Login attempt failed: User {Email} is already logged in", loginRequest.Email);
                        return Ok(new { message = "User is already logged in. Please logout first before logging in again." });
                    }
                    _logger.LogInformation("Existing token is invalid, proceeding with login for user: {Email}", loginRequest.Email);
                }

                var result = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);

                if (!result.Success)
                {
                    _logger.LogWarning("Login failed for user: {Email}. Reason: {Message}", loginRequest.Email, result.Message);
                    return Unauthorized(new { message = result.Message });
                }
                
                _logger.LogInformation("Login successful for user: {Email}", loginRequest.Email);

                // Set JWT token in HTTP-only cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps, // Only secure in HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = result.ExpiresAt
                };
                Response.Cookies.Append("jwt", result.Token, cookieOptions);

                return Ok(new
                {
                    success = result.Success,
                    user = result.User,
                    expiresAt = result.ExpiresAt,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for user: {Email}", loginRequest?.Email ?? "Unknown");
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        /// <summary>
        /// Validates the current user's token
        /// </summary>
        /// <returns>Current user information</returns>
        [HttpGet("me")]
        [AuthorizeCommon]
        public IActionResult GetCurrentUser()
        {
            _logger.LogInformation("GetCurrentUser request initiated");
            
            try
            {
                // Get user information from the authenticated context
                var userId = User.FindFirst("id")?.Value;
                var name = User.FindFirst("name")?.Value;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("GetCurrentUser failed: Invalid or missing user ID in token");
                    return Unauthorized(new { message = "Invalid token" });
                }
                
                _logger.LogInformation("GetCurrentUser successful for user: {Email} (ID: {UserId})", email, userId);

                var userInfo = new UserTokenInfoDTO
                {
                    Name = name ?? string.Empty,
                    Email = email ?? string.Empty
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetCurrentUser");
                return StatusCode(500, new { message = "An error occurred while getting user information" });
            }
        }

        /// <summary>
        /// Logs out the user by clearing the JWT cookie
        /// </summary>
        /// <returns>Logout confirmation</returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";

            if (userEmail == "Unknown")
                return Ok(new { message = "None user is logged." });
            _logger.LogInformation("Logout request initiated for user: {Email}", userEmail);
            
            try
            {
                // Clear the JWT cookie
                Response.Cookies.Delete("jwt");
                
                _logger.LogInformation("Logout successful for user: {Email}", userEmail);
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during logout for user: {Email}", userEmail);
                return StatusCode(500, new { message = "An error occurred during logout" });
            }
        }
    }
}