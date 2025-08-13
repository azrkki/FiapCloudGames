using FCG.Application.Services;
using FCG.Application.DTOs;
using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using FCG.Core.Interfaces;
using FCG.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Tests.Services
{
    [TestFixture]
    public class UserGameLibraryServiceTests
    {
        private Mock<IUserGameLibraryRepository> _mockUserGameLibraryRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IGameRepository> _mockGameRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ILogger<UserGameLibraryService>> _mockLogger;
        private UserGameLibraryService _userGameLibraryService;
        private UserGameLibrary _testUserGameLibrary;
        private User _testUser;
        private Game _testGame;
        private Role _testRole;

        [SetUp]
        public void Setup()
        {
            _mockUserGameLibraryRepository = new Mock<IUserGameLibraryRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockGameRepository = new Mock<IGameRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<UserGameLibraryService>>();

            _userGameLibraryService = new UserGameLibraryService(
                _mockUserGameLibraryRepository.Object,
                _mockUserRepository.Object,
                _mockGameRepository.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);

            // Setup test data
            _testRole = new Role("User");
            _testUser = new User("Test User", Email.Create("test@example.com"), Password.Create("Password123!"), _testRole);
            _testGame = new Game("Test Game", "Test Description", 59.99m);
            _testUserGameLibrary = new UserGameLibrary(_testUser, _testGame);
        }

        [Test]
        public void GetAllUserGameLibraries_ReturnsUserGameLibraryDTOs()
        {
            // Arrange
            var userGameLibraries = new List<UserGameLibrary> { _testUserGameLibrary };
            _mockUserGameLibraryRepository.Setup(r => r.GetAllWithUsersAndGames())
                .Returns(userGameLibraries);

            // Act
            var result = _userGameLibraryService.GetAllUserGameLibraries();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var libraryDto = result.First();
            Assert.AreEqual(_testUserGameLibrary.UserId, libraryDto.UserId);
            Assert.AreEqual(_testUserGameLibrary.GameId, libraryDto.GameId);
        }

        [Test]
        public async Task GetAllUserGameLibrariesAsync_ReturnsUserGameLibraryDTOs()
        {
            // Arrange
            var userGameLibraries = new List<UserGameLibrary> { _testUserGameLibrary };
            _mockUserGameLibraryRepository.Setup(r => r.GetAllWithUsersAndGamesAsync())
                .ReturnsAsync(userGameLibraries);

            // Act
            var result = await _userGameLibraryService.GetAllUserGameLibrariesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var libraryDto = result.First();
            Assert.AreEqual(_testUserGameLibrary.UserId, libraryDto.UserId);
            Assert.AreEqual(_testUserGameLibrary.GameId, libraryDto.GameId);
        }

        [Test]
        public void GetUserGameLibraryByUserIdAndGameId_ExistingLibrary_ReturnsUserGameLibraryDTO()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserIdAndGameId(userId, gameId))
                .Returns(_testUserGameLibrary);

            // Act
            var result = _userGameLibraryService.GetUserGameLibraryByUserIdAndGameId(userId, gameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testUserGameLibrary.UserId, result.UserId);
            Assert.AreEqual(_testUserGameLibrary.GameId, result.GameId);
        }

        [Test]
        public void GetUserGameLibraryByUserIdAndGameId_NonExistingLibrary_ReturnsNull()
        {
            // Arrange
            var userId = 999;
            var gameId = 999;
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserIdAndGameId(userId, gameId))
                .Returns((UserGameLibrary)null);

            // Act
            var result = _userGameLibraryService.GetUserGameLibraryByUserIdAndGameId(userId, gameId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserGameLibraryByUserIdAndGameIdAsync_ExistingLibrary_ReturnsUserGameLibraryDTO()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserIdAndGameIdAsync(userId, gameId))
                .ReturnsAsync(_testUserGameLibrary);

            // Act
            var result = await _userGameLibraryService.GetUserGameLibraryByUserIdAndGameIdAsync(userId, gameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testUserGameLibrary.UserId, result.UserId);
            Assert.AreEqual(_testUserGameLibrary.GameId, result.GameId);
        }

        [Test]
        public void GetUserGameLibrariesByUserId_ExistingUser_ReturnsUserGameLibraryDTOs()
        {
            // Arrange
            var userId = 1;
            var userGameLibraries = new List<UserGameLibrary> { _testUserGameLibrary };
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserId(userId))
                .Returns(userGameLibraries);

            // Act
            var result = _userGameLibraryService.GetUserGameLibrariesByUserId(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var libraryDto = result.First();
            Assert.AreEqual(_testUserGameLibrary.UserId, libraryDto.UserId);
        }

        [Test]
        public void GetUserGameLibrariesByUserId_NonExistingUser_ReturnsEmptyCollection()
        {
            // Arrange
            var userId = 999;
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserId(userId))
                .Returns(new List<UserGameLibrary>());

            // Act
            var result = _userGameLibraryService.GetUserGameLibrariesByUserId(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetUserGameLibrariesByGameId_ExistingGame_ReturnsUserGameLibraryDTOs()
        {
            // Arrange
            var gameId = 1;
            var userGameLibraries = new List<UserGameLibrary> { _testUserGameLibrary };
            _mockUserGameLibraryRepository.Setup(r => r.GetByGameId(gameId))
                .Returns(userGameLibraries);

            // Act
            var result = _userGameLibraryService.GetUserGameLibrariesByGameId(gameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var libraryDto = result.First();
            Assert.AreEqual(_testUserGameLibrary.GameId, libraryDto.GameId);
        }

        [Test]
        public void GetUserGameLibrariesByGameId_NonExistingGame_ReturnsEmptyCollection()
        {
            // Arrange
            var gameId = 999;
            _mockUserGameLibraryRepository.Setup(r => r.GetByGameId(gameId))
                .Returns(new List<UserGameLibrary>());

            // Act
            var result = _userGameLibraryService.GetUserGameLibrariesByGameId(gameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void AddGameToUserLibrary_ValidData_ReturnsUserGameLibraryDTO()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;

            _mockUserRepository.Setup(r => r.GetById(userId))
                .Returns(_testUser);
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGame);
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserIdAndGameId(userId, gameId))
                .Returns((UserGameLibrary)null);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _userGameLibraryService.AddGameToUserLibrary(userId, gameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(gameId, result.GameId);
            _mockUserGameLibraryRepository.Verify(r => r.Add(It.IsAny<UserGameLibrary>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddGameToUserLibrary_UserNotFound_ThrowsArgumentException()
        {
            // Arrange
            var userId = 999;
            var gameId = 1;

            _mockUserRepository.Setup(r => r.GetById(userId))
                .Returns((User)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _userGameLibraryService.AddGameToUserLibrary(userId, gameId));
            Assert.That(ex.Message, Does.Contain("not found"));
        }

        [Test]
        public void AddGameToUserLibrary_GameNotFound_ThrowsArgumentException()
        {
            // Arrange
            var userId = 1;
            var gameId = 999;

            _mockUserRepository.Setup(r => r.GetById(userId))
                .Returns(_testUser);
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns((Game)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _userGameLibraryService.AddGameToUserLibrary(userId, gameId));
            Assert.That(ex.Message, Does.Contain("not found"));
        }

        [Test]
        public void AddGameToUserLibrary_DuplicateEntry_ThrowsInvalidOperationException()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;

            _mockUserRepository.Setup(r => r.GetById(userId))
                .Returns(_testUser);
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGame);
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserIdAndGameId(userId, gameId))
                .Returns(_testUserGameLibrary);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _userGameLibraryService.AddGameToUserLibrary(userId, gameId));
            Assert.That(ex.Message, Does.Contain("already in user's library"));
        }

        [Test]
        public void RemoveGameFromUserLibrary_ExistingLibrary_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var gameId = 1;
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserIdAndGameId(userId, gameId))
                .Returns(_testUserGameLibrary);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _userGameLibraryService.RemoveGameFromUserLibrary(userId, gameId);

            // Assert
            Assert.IsTrue(result);
            _mockUserGameLibraryRepository.Verify(r => r.Remove(_testUserGameLibrary), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void RemoveGameFromUserLibrary_NonExistingLibrary_ReturnsFalse()
        {
            // Arrange
            var userId = 999;
            var gameId = 999;
            _mockUserGameLibraryRepository.Setup(r => r.GetByUserIdAndGameId(userId, gameId))
                .Returns((UserGameLibrary)null);

            // Act
            var result = _userGameLibraryService.RemoveGameFromUserLibrary(userId, gameId);

            // Assert
            Assert.IsFalse(result);
            _mockUserGameLibraryRepository.Verify(r => r.Remove(It.IsAny<UserGameLibrary>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Never);
        }
    }
}