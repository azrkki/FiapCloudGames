using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace FCG.Infrastructure.Tests.Repository
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private UserRepository _userRepository = null!;
        private Role _testRole = null!;
        private Mock<ILogger<UserRepository>> _mockLogger = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mockLogger = new Mock<ILogger<UserRepository>>();
            _userRepository = new UserRepository(_context, _mockLogger.Object);
            
            // Setup test data
            _testRole = new Role("User");
            _context.Roles.Add(_testRole);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Add_ValidUser_AddsUserToDatabase()
        {
            // Arrange
            var user = new User("Test User", Email.Create("test@example.com"), Password.Create("Password123!"), _testRole);

            // Act
            _userRepository.Add(user);
            _context.SaveChanges();

            // Assert
            var savedUser = _context.Users.FirstOrDefault(u => u.Email == "test@example.com");
            Assert.IsNotNull(savedUser);
            Assert.AreEqual("Test User", savedUser.Name);
        }

        [Test]
        public async Task GetByEmailAsync_ExistingEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User("Test User", Email.Create(email), Password.Create("Password123!"), _testRole);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByEmailAsync(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
            Assert.AreEqual("Test User", result.Name);
        }

        [Test]
        public async Task GetByEmailAsync_NonExistingEmail_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";

            // Act
            var result = await _userRepository.GetByEmailAsync(email);

            // Assert
            Assert.IsNull(result);
        }
    }
}