using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using FCG.Application.Services;
using FCG.Core.Interfaces;
using FCG.Core.Entity;
using FCG.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Tests.Services
{
    [TestFixture]
    public class GameServiceTests
    {
        private Mock<IGameRepository> _mockGameRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ILogger<GameService>> _mockLogger;
        private GameService _gameService;
        private Game _testGame;
        private Game _testGameOnSale;

        [SetUp]
        public void Setup()
        {
            _mockGameRepository = new Mock<IGameRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<GameService>>();

            _gameService = new GameService(
                _mockGameRepository.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);

            // Setup test data
            _testGame = new Game("Test Game", "Test Description", 29.99m);
            _testGameOnSale = new Game("Sale Game", "Sale Description", 39.99m);
            _testGameOnSale.ApplyDiscount(25);
            _testGameOnSale.UpdateIsOnSale(true);
        }

        [Test]
        public void GetAllGames_ReturnsGameDTOs()
        {
            // Arrange
            var games = new List<Game> { _testGame, _testGameOnSale };
            _mockGameRepository.Setup(r => r.GetAll())
                .Returns(games);

            // Act
            var result = _gameService.GetAllGames();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            var gameDto = result.First();
            Assert.AreEqual(_testGame.Name, gameDto.Name);
            Assert.AreEqual(_testGame.Description, gameDto.Description);
            Assert.AreEqual(_testGame.Price, gameDto.Price);
        }

        [Test]
        public async Task GetAllGamesAsync_ReturnsGameDTOs()
        {
            // Arrange
            var games = new List<Game> { _testGame, _testGameOnSale };
            _mockGameRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(games);

            // Act
            var result = await _gameService.GetAllGamesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            var gameDto = result.First();
            Assert.AreEqual(_testGame.Name, gameDto.Name);
            Assert.AreEqual(_testGame.Description, gameDto.Description);
        }

        [Test]
        public void GetGameById_ExistingGame_ReturnsGameDTO()
        {
            // Arrange
            var gameId = 1;
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGame);

            // Act
            var result = _gameService.GetGameById(gameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testGame.Name, result.Name);
            Assert.AreEqual(_testGame.Description, result.Description);
            Assert.AreEqual(_testGame.Price, result.Price);
        }

        [Test]
        public void GetGameById_NonExistingGame_ReturnsNull()
        {
            // Arrange
            var gameId = 999;
            _mockGameRepository.Setup(r => r.GetByIdWithUsers(gameId))
                .Returns((Game)null);

            // Act
            var result = _gameService.GetGameById(gameId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetGamesOnSale_ReturnsOnlyDiscountedGames()
        {
            // Arrange
            var gamesOnSale = new List<Game> { _testGameOnSale };
            _mockGameRepository.Setup(r => r.GetGamesOnSale())
                .Returns(gamesOnSale);

            // Act
            var result = _gameService.GetGamesOnSale();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var gameDto = result.First();
            Assert.AreEqual(_testGameOnSale.Name, gameDto.Name);
            Assert.IsTrue(gameDto.IsOnSale);
            Assert.Greater(gameDto.Discount, 0);
        }

        [Test]
        public async Task GetGamesOnSaleAsync_ReturnsOnlyDiscountedGames()
        {
            // Arrange
            var gamesOnSale = new List<Game> { _testGameOnSale };
            _mockGameRepository.Setup(r => r.GetGamesOnSaleAsync())
                .ReturnsAsync(gamesOnSale);

            // Act
            var result = await _gameService.GetGamesOnSaleAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var gameDto = result.First();
            Assert.AreEqual(_testGameOnSale.Name, gameDto.Name);
            Assert.IsTrue(gameDto.IsOnSale);
        }

        [Test]
        public void CreateGame_ValidData_ReturnsGameDTO()
        {
            // Arrange
            var gameCreateDto = new GameCreateDTO
            {
                Name = "New Game",
                Description = "New Description",
                Value = 49.99m
            };

            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _gameService.CreateGame(gameCreateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(gameCreateDto.Name, result.Name);
            Assert.AreEqual(gameCreateDto.Description, result.Description);
            Assert.AreEqual(gameCreateDto.Value, result.Price);
            _mockGameRepository.Verify(r => r.Add(It.IsAny<Game>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public async Task CreateGameAsync_ValidData_ReturnsGameDTO()
        {
            // Arrange
            var gameCreateDto = new GameCreateDTO
            {
                Name = "New Game Async",
                Description = "New Description Async",
                Value = 59.99m
            };

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _gameService.CreateGameAsync(gameCreateDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(gameCreateDto.Name, result.Name);
            Assert.AreEqual(gameCreateDto.Description, result.Description);
            _mockGameRepository.Verify(r => r.AddAsync(It.IsAny<Game>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateGame_ValidData_ReturnsUpdatedGameDTO()
        {
            // Arrange
            var gameUpdateDto = new GameUpdateDTO
            {
                Id = 1,
                Name = "Updated Game",
                Description = "Updated Description",
                Price = 39.99m,
                Discount = 10,
                IsOnSale = true
            };

            _mockGameRepository.Setup(r => r.GetById(1))
                .Returns(_testGame);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _gameService.UpdateGame(gameUpdateDto);

            // Assert
            Assert.IsNotNull(result);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void UpdateGame_NonExistingGame_ThrowsException()
        {
            // Arrange
            var gameUpdateDto = new GameUpdateDTO
            {
                Id = 999,
                Name = "Updated Game",
                Description = "Updated Description"
            };

            _mockGameRepository.Setup(r => r.GetByIdWithUsers(999))
                .Returns((Game)null);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _gameService.UpdateGame(gameUpdateDto));
            Assert.AreEqual("Game ID 999 not found", ex.Message);
        }

        [Test]
        public void DeleteGame_ExistingGame_ReturnsTrue()
        {
            // Arrange
            var gameId = 1;
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGame);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _gameService.DeleteGame(gameId);

            // Assert
            Assert.IsTrue(result);
            _mockGameRepository.Verify(r => r.Remove(_testGame), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteGame_NonExistingGame_ReturnsFalse()
        {
            // Arrange
            var gameId = 999;
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns((Game)null);

            // Act
            var result = _gameService.DeleteGame(gameId);

            // Assert
            Assert.IsFalse(result);
            _mockGameRepository.Verify(r => r.Remove(It.IsAny<Game>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Test]
        public void ApplyDiscount_ValidGame_ReturnsUpdatedGameDTO()
        {
            // Arrange
            var gameId = 1;
            var discountPercentage = 20;
            
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGame);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _gameService.ApplyDiscount(gameId, discountPercentage);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(discountPercentage, result.Discount);
            Assert.IsTrue(result.IsOnSale);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void RemoveDiscount_ValidGame_ReturnsUpdatedGameDTO()
        {
            // Arrange
            var gameId = 1;
            
            _mockGameRepository.Setup(r => r.GetById(gameId))
                .Returns(_testGameOnSale);
            _mockUnitOfWork.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = _gameService.RemoveDiscount(gameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Discount);
            Assert.IsFalse(result.IsOnSale);
            _mockUnitOfWork.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Test]
        public void ApplyDiscount_NonExistingGame_ThrowsException()
        {
            // Arrange
            var gameId = 999;
            var discountPercentage = 20;
            
            _mockGameRepository.Setup(r => r.GetByIdWithUsers(gameId))
                .Returns((Game)null);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _gameService.ApplyDiscount(gameId, discountPercentage));
            Assert.AreEqual("Game ID 999 not found", ex.Message);
        }
    }
}