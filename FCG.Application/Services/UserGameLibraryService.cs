using FCG.Application.DTOs;
using FCG.Core.Entity;
using FCG.Core.Interfaces;
using FCG.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class UserGameLibraryService : IUserGameLibraryService
    {
        private readonly IUserGameLibraryRepository _userGameLibraryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserGameLibraryService> _logger;

        public UserGameLibraryService(
            IUserGameLibraryRepository userGameLibraryRepository,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IUnitOfWork unitOfWork,
            ILogger<UserGameLibraryService> logger)
        {
            _userGameLibraryRepository = userGameLibraryRepository ?? throw new ArgumentNullException(nameof(userGameLibraryRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public UserGameLibraryDTO AddGameToUserLibrary(int userId, int gameId)
        {
            _logger.LogInformation("AddGameToUserLibrary called for userId: {UserId}, gameId: {GameId}", userId, gameId);
            
            try
            {
                // Check if user already exists
                var user = _userRepository.GetById(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found with id: {UserId}", userId);
                    throw new ArgumentException($"User ID {userId} not found.");
                }

                // Check if game already exists
                var game = _gameRepository.GetById(gameId);
                if (game == null)
                {
                    _logger.LogWarning("Game not found with id: {GameId}", gameId);
                    throw new ArgumentException($"Game ID {gameId} not found.");
                }

                // Check if the game is already in the User's library
            var existingLibrary = _userGameLibraryRepository.GetByUserIdAndGameId(userId, gameId);
            if (existingLibrary != null)
                throw new InvalidOperationException($"Game is already in user's library.");

            // Add a game into a User's library
            var userGameLibrary = new UserGameLibrary
            {
                UserId = userId,
                GameId = gameId
            };

                _userGameLibraryRepository.Add(userGameLibrary);
                _unitOfWork.SaveChanges();

                _logger.LogInformation("Successfully added game {GameId} to user {UserId} library", gameId, userId);
                return MapToDTO(userGameLibrary, user.Name, game.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding game {GameId} to user {UserId} library", gameId, userId);
                throw;
            }
        }

        public async Task<UserGameLibraryDTO> AddGameToUserLibraryAsync(int userId, int gameId)
        {
            // Check if user already exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User ID {userId} not found.");

            // Check if game already exists
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
                throw new ArgumentException($"Game ID {gameId} not found.");

            // Check if the game is already in the User's library
            var existingLibrary = await _userGameLibraryRepository.GetByUserIdAndGameIdAsync(userId, gameId);
            if (existingLibrary != null)
                throw new InvalidOperationException($"Game is already in user's library.");

            // Add a game into a User's library
            var userGameLibrary = new UserGameLibrary
            {
                UserId = userId,
                GameId = gameId
            };

            await _userGameLibraryRepository.AddAsync(userGameLibrary);
            await _unitOfWork.SaveChangesAsync();

            return MapToDTO(userGameLibrary, user.Name, game.Name);
        }

        public IEnumerable<UserGameLibraryDTO> GetAllUserGameLibraries()
        {
            var libraries = _userGameLibraryRepository.GetAllWithUsersAndGames();
            return libraries.Select(l => MapToDTO(l, l.User?.Name, l.Game?.Name));
        }

        public async Task<IEnumerable<UserGameLibraryDTO>> GetAllUserGameLibrariesAsync()
        {
            var libraries = await _userGameLibraryRepository.GetAllWithUsersAndGamesAsync();
            return libraries.Select(l => MapToDTO(l, l.User?.Name, l.Game?.Name));
        }

        public IEnumerable<UserGameLibraryDTO> GetUserGameLibrariesByGameId(int gameId)
        {
            var libraries = _userGameLibraryRepository.GetByGameId(gameId);
            return libraries.Select(l => MapToDTO(l, l.User?.Name, l.Game?.Name));
        }

        public async Task<IEnumerable<UserGameLibraryDTO>> GetUserGameLibrariesByGameIdAsync(int gameId)
        {
            var libraries = await _userGameLibraryRepository.GetByGameIdAsync(gameId);
            return libraries.Select(l => MapToDTO(l, l.User?.Name, l.Game?.Name));
        }

        public IEnumerable<UserGameLibraryDTO> GetUserGameLibrariesByUserId(int userId)
        {
            var libraries = _userGameLibraryRepository.GetByUserId(userId);
            return libraries.Select(l => MapToDTO(l, l.User?.Name, l.Game?.Name));
        }

        public async Task<IEnumerable<UserGameLibraryDTO>> GetUserGameLibrariesByUserIdAsync(int userId)
        {
            var libraries = await _userGameLibraryRepository.GetByUserIdAsync(userId);
            return libraries.Select(l => MapToDTO(l, l.User?.Name, l.Game?.Name));
        }

        public UserGameLibraryDTO GetUserGameLibraryByUserIdAndGameId(int userId, int gameId)
        {
            var library = _userGameLibraryRepository.GetByUserIdAndGameId(userId, gameId);
            if (library == null)
                return null;

            return MapToDTO(library, library.User?.Name, library.Game?.Name);
        }

        public async Task<UserGameLibraryDTO> GetUserGameLibraryByUserIdAndGameIdAsync(int userId, int gameId)
        {
            var library = await _userGameLibraryRepository.GetByUserIdAndGameIdAsync(userId, gameId);
            if (library == null)
                return null;

            return MapToDTO(library, library.User?.Name, library.Game?.Name);
        }

        public bool RemoveGameFromUserLibrary(int userId, int gameId)
        {
            var library = _userGameLibraryRepository.GetByUserIdAndGameId(userId, gameId);
            if (library == null)
                return false;

            _userGameLibraryRepository.Remove(library);
            _unitOfWork.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveGameFromUserLibraryAsync(int userId, int gameId)
        {
            var library = await _userGameLibraryRepository.GetByUserIdAndGameIdAsync(userId, gameId);
            if (library == null)
                return false;

            _userGameLibraryRepository.Remove(library);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private UserGameLibraryDTO MapToDTO(UserGameLibrary userGameLibrary, string userName, string gameName)
        {
            return new UserGameLibraryDTO
            {
                UserId = userGameLibrary.UserId,
                GameId = userGameLibrary.GameId,
                UserName = userName,
                GameName = gameName
            };
        }
    }
}