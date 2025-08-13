using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using FCG.Application.Services;
using FCG.Core.Interfaces;
using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using FCG.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IRoleRepository> _mockRoleRepository;
        private Mock<IGameRepository> _mockGameRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ILogger<UserService>> _mockLogger;
        private UserService _userService;
        private User _testUser;
        private Role _testRole;
        private Game _testGame;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRoleRepository = new Mock<IRoleRepository>();
            _mockGameRepository = new Mock<IGameRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockRoleRepository.Object,
                _mockGameRepository.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);

            // Setup test data
            _testRole = new Role("User");
            _testUser = new User("Test User", Email.Create("test@example.com"), Password.Create("Password123!"), _testRole);
            _testGame = new Game("Test Game", "Test Description", 29.99m);
        }

        [Test]
        public void GetAllUsers_ReturnsUserDTOs()
        {
            // Arrange
            var users = new List<User> { _testUser };
            _mockUserRepository.Setup(r => r.GetAllWithRolesAndGames())
                .Returns(users);

            // Act
            var result = _userService.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var userDto = result.First();
            Assert.AreEqual(_testUser.Name, userDto.Name);
            Assert.AreEqual(_testUser.Email, userDto.Email);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsUserDTOs()
        {
            // Arrange
            var users = new List<User> { _testUser };
            _mockUserRepository.Setup(r => r.GetAllWithRolesAndGamesAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var userDto = result.First();
            Assert.AreEqual(_testUser.Name, userDto.Name);
            Assert.AreEqual(_testUser.Email, userDto.Email);
        }

        [Test]
        public void GetUserById_ExistingUser_ReturnsUserDTO()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(r => r.GetByIdWithRoleAndGames(userId))
                .Returns(_testUser);

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testUser.Name, result.Name);
            Assert.AreEqual(_testUser.Email, result.Email);
        }

        [Test]
        public void GetUserById_NonExistingUser_ReturnsNull()
        {
            // Arrange
            var userId = 999;
            _mockUserRepository.Setup(r => r.GetByIdWithRoleAndGames(userId))
                .Returns((User)null);

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetUserByEmail_ExistingUser_ReturnsUserDTO()
        {
            // Arrange
            var email = "test@example.com";
            _mockUserRepository.Setup(r => r.GetByEmail(email))
                .Returns(_testUser);

            // Act
            var result = _userService.GetUserByEmail(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testUser.Name, result.Name);
            Assert.AreEqual(_testUser.Email, result.Email);
        }

        [Test]
        public void CreateUser_ValidData_ReturnsUserDTO()
        {
            // Arrange
            var userCreateDto = new UserCreateDTO
            {
                Name = "New User",
                Email = "newuser@example.com",
                Password = "Password123!",
                RoleId = 1
            };

            _mockRoleRepository.Setup(r => r.GetById(1))
                .Returns(_testRole);
            _mockUserRepository.Setup(r => r.GetByEmail(userCreateDto.Email))
                .Returns((User)null);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _userService.CreateUser(userCreateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userCreateDto.Name, result.Name);
            Assert.AreEqual(userCreateDto.Email, result.Email);
            _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void CreateUser_ExistingEmail_ThrowsException()
        {
            // Arrange
            var userCreateDto = new UserCreateDTO
            {
                Name = "New User",
                Email = "test@example.com",
                Password = "Password123!",
                RoleId = 1
            };

            _mockUserRepository.Setup(r => r.Any(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(true);
            _mockRoleRepository.Setup(r => r.GetById(1))
                .Returns(_testRole);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _userService.CreateUser(userCreateDto));
            Assert.AreEqual("Email already in use", ex.Message);
        }

        [Test]
        public void CreateUser_InvalidRole_ThrowsException()
        {
            // Arrange
            var userCreateDto = new UserCreateDTO
            {
                Name = "New User",
                Email = "newuser@example.com",
                Password = "Password123!",
                RoleId = 999
            };

            _mockRoleRepository.Setup(r => r.GetById(999))
                .Returns((Role)null);
            _mockUserRepository.Setup(r => r.GetByEmail(userCreateDto.Email))
                .Returns((User)null);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _userService.CreateUser(userCreateDto));
            Assert.AreEqual("Role ID 999 not found", ex.Message);
        }

        [Test]
        public void UpdateUser_ValidData_ReturnsUpdatedUserDTO()
        {
            // Arrange
            var userUpdateDto = new UserUpdateDTO
            {
                Id = 1,
                Name = "Updated User",
                Email = "updated@example.com",
                RoleId = 1
            };

            _mockUserRepository.Setup(r => r.GetById(1))
                .Returns(_testUser);
            _mockUserRepository.Setup(r => r.Any(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(false);
            _mockRoleRepository.Setup(r => r.GetById(1))
                .Returns(_testRole);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _userService.UpdateUser(userUpdateDto);

            // Assert
            Assert.IsNotNull(result);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteUser_ExistingUser_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(r => r.GetById(userId))
                .Returns(_testUser);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _userService.DeleteUser(userId);

            // Assert
            Assert.IsTrue(result);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteUser_NonExistingUser_ReturnsFalse()
        {
            // Arrange
            var userId = 999;
            _mockUserRepository.Setup(r => r.GetById(userId))
                .Returns((User)null);

            // Act
            var result = _userService.DeleteUser(userId);

            // Assert
            Assert.IsFalse(result);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Test]
        public void AddGameToUserLibrary_ValidData_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;
            
            _mockUserRepository.Setup(r => r.GetByIdWithRoleAndGames(userId))
                .Returns(_testUser);
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGame);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _userService.AddGameToUserLibrary(userId, gameId);

            // Assert
            Assert.IsTrue(result);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void RemoveGameFromUserLibrary_ValidData_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;
            
            // Add game to user's library first
            _testUser.AddGameToLibrary(_testGame);
            
            _mockUserRepository.Setup(r => r.GetByIdWithRoleAndGames(userId))
                .Returns(_testUser);
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGame);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _userService.RemoveGameFromUserLibrary(userId, gameId);

            // Assert
            Assert.IsTrue(result);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }
    }
}