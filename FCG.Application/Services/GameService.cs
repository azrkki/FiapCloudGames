using FCG.Application.DTOs;
using FCG.Core.Entity;
using FCG.Core.Interfaces;
using FCG.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GameService> _logger;

        public GameService(IGameRepository gameRepository, IUnitOfWork unitOfWork, ILogger<GameService> logger)
        {
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<GameDTO> GetAllGames()
        {
            _logger.LogInformation("GetAllGames called");
            try
            {
                var games = _gameRepository.GetAll();
                var result = games.Select(MapGameToDto).ToList();
                _logger.LogInformation("Successfully retrieved {Count} games", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all games");
                throw;
            }
        }

        public async Task<IEnumerable<GameDTO>> GetAllGamesAsync()
        {
            _logger.LogInformation("GetAllGamesAsync called");
            try
            {
                var games = await _gameRepository.GetAllAsync();
                var result = games.Select(MapGameToDto).ToList();
                _logger.LogInformation("Successfully retrieved {Count} games asynchronously", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all games asynchronously");
                throw;
            }
        }

        public GameDTO GetGameById(int id)
        {
            _logger.LogInformation("GetGameById called with id: {GameId}", id);
            try
            {
                var game = _gameRepository.GetById(id);
                if (game != null)
                {
                    _logger.LogInformation("Successfully retrieved game: {GameName} (ID: {GameId})", game.Name, id);
                    return MapGameToDto(game);
                }
                else
                {
                    _logger.LogWarning("Game not found with id: {GameId}", id);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving game with id: {GameId}", id);
                throw;
            }
        }

        public async Task<GameDTO> GetGameByIdAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            return game != null ? MapGameToDto(game) : null;
        }

        public IEnumerable<GameDTO> GetGamesOnSale()
        {
            var games = _gameRepository.GetGamesOnSale();
            return games.Select(MapGameToDto);
        }

        public async Task<IEnumerable<GameDTO>> GetGamesOnSaleAsync()
        {
            var games = await _gameRepository.GetGamesOnSaleAsync();
            return games.Select(MapGameToDto);
        }

        public GameDTO CreateGame(GameCreateDTO gameDto)
        {
            if (gameDto == null)
                throw new ArgumentNullException(nameof(gameDto));

            var game = new Game(gameDto.Name, gameDto.Description, gameDto.Value);

            _gameRepository.Add(game);
            _unitOfWork.SaveChanges();

            return MapGameToDto(game);
        }

        public async Task<GameDTO> CreateGameAsync(GameCreateDTO gameDto)
        {
            if (gameDto == null)
                throw new ArgumentNullException(nameof(gameDto));

            var game = new Game(gameDto.Name, gameDto.Description, gameDto.Value);

            await _gameRepository.AddAsync(game);
            await _unitOfWork.SaveChangesAsync();

            return MapGameToDto(game);
        }

        public GameDTO UpdateGame(GameUpdateDTO gameDto)
        {
            if (gameDto == null)
                throw new ArgumentNullException(nameof(gameDto));

            var game = _gameRepository.GetById(gameDto.Id);
            if (game == null)
                throw new InvalidOperationException($"Game ID {gameDto.Id} not found");

            // Update Details
            game.UpdateDetails(gameDto.Name, gameDto.Description);

            // Update Price
            if (gameDto.Price.HasValue)
                game.UpdatePrice(gameDto.Price.Value);

            // Update Discount
            if (gameDto.Discount.HasValue)
                game.ApplyDiscount(gameDto.Discount.Value);

            if (gameDto.IsOnSale != null)
                game.UpdateIsOnSale(gameDto.IsOnSale);

            _gameRepository.Update(game);
            _unitOfWork.SaveChanges();

            return MapGameToDto(game);
        }

        public async Task<GameDTO> UpdateGameAsync(GameUpdateDTO gameDto)
        {
            if (gameDto == null)
                throw new ArgumentNullException(nameof(gameDto));

            var game = await _gameRepository.GetByIdAsync(gameDto.Id);
            if (game == null)
                throw new InvalidOperationException($"Game ID {gameDto.Id} not found");

            // Update Details
            game.UpdateDetails(gameDto.Name, gameDto.Description);

            // Update Price
            if (gameDto.Price.HasValue)
                game.UpdatePrice(gameDto.Price.Value);

            // Update Discount
            if (gameDto.Discount.HasValue)
                game.ApplyDiscount(gameDto.Discount.Value);

            _gameRepository.Update(game);
            await _unitOfWork.SaveChangesAsync();

            return MapGameToDto(game);
        }

        public bool DeleteGame(int id)
        {
            var game = _gameRepository.GetById(id);
            if (game == null)
                return false;

            _gameRepository.Remove(game);
            _unitOfWork.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
                return false;

            _gameRepository.Remove(game);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public GameDTO ApplyDiscount(int id, int discountPercentage)
        {
            var game = _gameRepository.GetById(id);
            if (game == null)
                throw new InvalidOperationException($"Game ID {id} not found");

            game.ApplyDiscount(discountPercentage);
            _gameRepository.Update(game);
            _unitOfWork.SaveChanges();

            return MapGameToDto(game);
        }

        public async Task<GameDTO> ApplyDiscountAsync(int id, int discountPercentage)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
                throw new InvalidOperationException($"Game ID {id} not found");

            game.ApplyDiscount(discountPercentage);
            _gameRepository.Update(game);
            await _unitOfWork.SaveChangesAsync();

            return MapGameToDto(game);
        }

        public GameDTO RemoveDiscount(int id)
        {
            var game = _gameRepository.GetById(id);
            if (game == null)
                throw new InvalidOperationException($"Game ID {id} not found");

            game.RemoveDiscount();
            _gameRepository.Update(game);
            _unitOfWork.SaveChanges();

            return MapGameToDto(game);
        }

        public async Task<GameDTO> RemoveDiscountAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
                throw new InvalidOperationException($"Game ID {id} not found");

            game.RemoveDiscount();
            _gameRepository.Update(game);
            await _unitOfWork.SaveChangesAsync();

            return MapGameToDto(game);
        }

        public GameDTO UpdateSaleStatus(int id, bool isOnSale)
        {
            var game = _gameRepository.GetById(id);
            if (game == null)
                return null;

            game.UpdateIsOnSale(isOnSale);
            _gameRepository.Update(game);
            _unitOfWork.SaveChanges();

            return MapGameToDto(game);
        }

        public async Task<GameDTO> UpdateSaleStatusAsync(int id, bool isOnSale)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
                return null;

            game.UpdateIsOnSale(isOnSale);
            _gameRepository.Update(game);
            await _unitOfWork.SaveChangesAsync();

            return MapGameToDto(game);
        }

        // Auxiliar method to map Game to GameDTO
        private GameDTO MapGameToDto(Game game)
        {
            if (game == null)
                return null;

            return new GameDTO
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                OriginalPrice = game.OriginalPrice,
                Discount = game.Discount,
                Price = game.Price,
                IsOnSale = game.IsOnSale
            };
        }
    }
}