using FCG.Api.Controllers;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FCG.Api.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService = null!;
        private Mock<ILogger<AuthController>> _mockLogger = null!;
        private AuthController _authController = null!;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_mockAuthService.Object, _mockLogger.Object);
            
            // Setup HTTP context to avoid null reference exceptions
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Authorization", "");
            _authController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginRequest = new LoginRequestDTO { Email = "test@example.com", Password = "password123" };
            var authResult = new AuthResultDTO
            {
                Success = true,
                Token = "test-token",
                Message = "Login successful",
                User = new UserAuthResponseDTO { Name = "Test User", Email = "test@example.com" },
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            
            _mockAuthService.Setup(x => x.LoginAsync("test@example.com", "password123"))
                           .ReturnsAsync(authResult);

            // Act
            var result = await _authController.Login(loginRequest);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDTO { Email = "test@example.com", Password = "wrongpassword" };
            var authResult = new AuthResultDTO
            {
                Success = false,
                Token = null,
                Message = "Invalid credentials",
                User = null,
                ExpiresAt = DateTime.MinValue
            };
            
            _mockAuthService.Setup(x => x.LoginAsync("test@example.com", "wrongpassword"))
                           .ReturnsAsync(authResult);

            // Act
            var result = await _authController.Login(loginRequest);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(401, objectResult.StatusCode);
        }
    }
}