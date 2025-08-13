using FCG.Application.DTOs;
using System.Threading.Tasks;

namespace FCG.Application.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>Authentication result with JWT token if successful</returns>
        Task<AuthResultDTO> LoginAsync(string email, string password);

        /// <summary>
        /// Validates a JWT token.
        /// </summary>
        /// <param name="token">JWT token to validate</param>
        /// <returns>True if token is valid</returns>
        bool ValidateToken(string token);

        /// <summary>
        /// Gets user information from JWT token.
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>User information</returns>
        UserTokenInfoDTO GetUserFromToken(string token);
    }
}