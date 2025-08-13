using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FCG.Application.Services;
using FCG.Core.Interfaces;
using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using FCG.Application.DTOs;
using System.Threading.Tasks;

namespace FCG.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<AuthService>> _mockLogger;
        private AuthService _authService;
        private User _testUser;
        private Role _testRole;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            // Setup configuration mock
            _mockConfiguration.Setup(c => c["Jwt:Secret"]).Returns("your-256-bit-secret-key-here-for-testing-purposes-only");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("FCG.Api");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("FCG.Users");
            _mockConfiguration.Setup(c => c["Jwt:ExpirationMinutes"]).Returns("60");

            _authService = new AuthService(_mockUserRepository.Object, _mockConfiguration.Object, _mockLogger.Object);

            // Setup test data
            _testRole = new Role("User");
            _testRole.Id = 1; // Set a valid role ID
            _testUser = new User("Test User", Email.Create("test@example.com"), Password.Create("Password123!"), _testRole);
            _testUser.Id = 1; // Set a valid user ID
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccessResult()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            
            _mockUserRepository.Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(_testUser);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Token);
            Assert.IsNotNull(result.User);
            Assert.AreEqual(_testUser.Name, result.User.Name);
            Assert.AreEqual(_testUser.Email, result.User.Email);
        }

        [Test]
        public async Task LoginAsync_InvalidEmail_ReturnsFailureResult()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "Password123!";
            
            _mockUserRepository.Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Token);
            Assert.AreEqual("Invalid email or password.", result.Message);
        }

        [Test]
        public async Task LoginAsync_InvalidPassword_ReturnsFailureResult()
        {
            // Arrange
            var email = "test@example.com";
            var password = "WrongPassword";
            
            _mockUserRepository.Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync(_testUser);

            // Act
            var result = await _authService.LoginAsync(email, password);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Token);
            Assert.AreEqual("Invalid email or password.", result.Message);
        }

        [Test]
        public void ValidateToken_ValidToken_ReturnsTrue()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com"))
                .ReturnsAsync(_testUser);
            var loginResult = _authService.LoginAsync("test@example.com", "Password123!").Result;

            // Act
            var isValid = _authService.ValidateToken(loginResult.Token);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ValidateToken_InvalidToken_ReturnsFalse()
        {
            // Arrange
            var invalidToken = "invalid.token.here";

            // Act
            var isValid = _authService.ValidateToken(invalidToken);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void ValidateToken_NullToken_ReturnsFalse()
        {
            // Act
            var isValid = _authService.ValidateToken(null);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void ValidateToken_EmptyToken_ReturnsFalse()
        {
            // Act
            var isValid = _authService.ValidateToken(string.Empty);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void GetUserFromToken_ValidToken_ReturnsUserInfo()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com"))
                .ReturnsAsync(_testUser);
            
            var loginResult = _authService.LoginAsync("test@example.com", "Password123!").Result;
            
            // Verify login was successful and token exists
            Assert.IsTrue(loginResult.Success, "Login should be successful");
            Assert.IsNotNull(loginResult.Token, "Token should not be null");
            Assert.IsNotEmpty(loginResult.Token, "Token should not be empty");
            
            // Debug: Print the token
            Console.WriteLine($"Generated token: {loginResult.Token}");
            
            // Verify token validation works
            var isValidToken = _authService.ValidateToken(loginResult.Token);
            Assert.IsTrue(isValidToken, "Token should be valid");

            // Act
            var userInfo = _authService.GetUserFromToken(loginResult.Token);

            // Assert
            Assert.IsNotNull(userInfo, "UserInfo should not be null");
            Assert.AreEqual(_testUser.Name, userInfo.Name);
            Assert.AreEqual(_testUser.Email, userInfo.Email);
        }

        [Test]
        public void GetUserFromToken_InvalidToken_ReturnsNull()
        {
            // Arrange
            var invalidToken = "invalid.token.here";

            // Act
            var userInfo = _authService.GetUserFromToken(invalidToken);

            // Assert
            Assert.IsNull(userInfo);
        }
    }
}