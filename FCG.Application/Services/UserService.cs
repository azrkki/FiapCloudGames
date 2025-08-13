using FCG.Application.DTOs;
using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using FCG.Core.Interfaces;
using FCG.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, 
                          IGameRepository gameRepository, IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers called");
            try
            {
                var users = _userRepository.GetAllWithRolesAndGames();
                var result = users.Select(MapUserToDto).ToList();
                _logger.LogInformation("Successfully retrieved {Count} users", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            _logger.LogInformation("GetAllUsersAsync called");
            try
            {
                var users = await _userRepository.GetAllWithRolesAndGamesAsync();
                var result = users.Select(MapUserToDto).ToList();
                _logger.LogInformation("Successfully retrieved {Count} users asynchronously", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users asynchronously");
                throw;
            }
        }

        public UserDTO GetUserById(int id)
        {
            var user = _userRepository.GetByIdWithRoleAndGames(id);
            return user != null ? MapUserToDto(user) : null;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdWithRoleAndGamesAsync(id);
            return user != null ? MapUserToDto(user) : null;
        }

        public UserDTO GetUserByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            return user != null ? MapUserToDto(user) : null;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? MapUserToDto(user) : null;
        }

        public UserDTO CreateUser(UserCreateDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            // Check if email already exists
            if (_userRepository.Any(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already in use");

            // Get Role
            var role = _roleRepository.GetById(userDto.RoleId);
            if (role == null)
                throw new InvalidOperationException($"Role ID {userDto.RoleId} not found");

            // Create ValueObjects
            var email = Email.Create(userDto.Email);
            var password = Password.Create(userDto.Password);

            // Create user
            var user = new User(userDto.Name, email, password, role);

            // Add user
            _userRepository.Add(user);
            _unitOfWork.SaveChanges();

            return MapUserToDto(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserCreateDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            // Check if email already exists
            if (await _userRepository.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already in use");

            // Get role
            var role = await _roleRepository.GetByIdAsync(userDto.RoleId);
            if (role == null)
                throw new InvalidOperationException($"Role ID {userDto.RoleId} not found");

            // Create ValueObject
            var email = Email.Create(userDto.Email);
            var password = Password.Create(userDto.Password);

            // Create user
            var user = new User(userDto.Name, email, password, role);

            // Add user
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return MapUserToDto(user);
        }

        public UserDTO UpdateUser(UserUpdateDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            var user = _userRepository.GetById(userDto.Id);
            if (user == null)
                throw new InvalidOperationException($"User ID {userDto.Id} not found");

            // Check if email already exists (if changing)
            if (!string.IsNullOrEmpty(userDto.Email) && userDto.Email != user.Email &&
                _userRepository.Any(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already in use");

            // Update role if necessary
            if (userDto.RoleId.HasValue && userDto.RoleId.Value != user.RoleId)
            {
                var role = _roleRepository.GetById(userDto.RoleId.Value);
                if (role == null)
                    throw new InvalidOperationException($"Role ID {userDto.RoleId.Value} not found");

                user.ChangeRole(role);
            }

            // Update personal information
            var email = !string.IsNullOrEmpty(userDto.Email) ? Email.Create(userDto.Email) : null;
            user.UpdatePersonalInfo(userDto.Name, email);

            // Save changes
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();

            return MapUserToDto(user);
        }

        public async Task<UserDTO> UpdateUserAsync(UserUpdateDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            var user = await _userRepository.GetByIdAsync(userDto.Id);
            if (user == null)
                throw new InvalidOperationException($"User ID {userDto.Id} not found");

            // Check if email already exists (if changing)
            if (!string.IsNullOrEmpty(userDto.Email) && userDto.Email != user.Email &&
                await _userRepository.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Email already in use");

            // Update Role if necessary
            if (userDto.RoleId.HasValue && userDto.RoleId.Value != user.RoleId)
            {
                var role = await _roleRepository.GetByIdAsync(userDto.RoleId.Value);
                if (role == null)
                    throw new InvalidOperationException($"Role ID {userDto.RoleId.Value} not found");

                user.ChangeRole(role);
            }

            // Update personal information
            var email = !string.IsNullOrEmpty(userDto.Email) ? Email.Create(userDto.Email) : null;
            user.UpdatePersonalInfo(userDto.Name, email);

            // Save changes
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return MapUserToDto(user);
        }

        public bool DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return false;

            // Update RemoveFlag
            user.MarkForRemoval();
            _userRepository.Update(user);
            _unitOfWork.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            // Update RemoveFlag
            user.MarkForRemoval();
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public bool AddGameToUserLibrary(int userId, int gameId)
        {
            var user = _userRepository.GetByIdWithRoleAndGames(userId);
            if (user == null)
                return false;

            var game = _gameRepository.GetById(gameId);
            if (game == null)
                return false;

            try
            {
                user.AddGameToLibrary(game);
                _userRepository.Update(user);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                // Game is already in the library
                return false;
            }
        }

        public async Task<bool> AddGameToUserLibraryAsync(int userId, int gameId)
        {
            var user = await _userRepository.GetByIdWithRoleAndGamesAsync(userId);
            if (user == null)
                return false;

            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
                return false;

            try
            {
                user.AddGameToLibrary(game);
                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                // Game is already in the library
                return false;
            }
        }

        public bool RemoveGameFromUserLibrary(int userId, int gameId)
        {
            var user = _userRepository.GetByIdWithRoleAndGames(userId);
            if (user == null)
                return false;

            var game = _gameRepository.GetById(gameId);
            if (game == null)
                return false;

            try
            {
                user.RemoveGameFromLibrary(game);
                _userRepository.Update(user);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                // Game is already in the library
                return false;
            }
        }

        public async Task<bool> RemoveGameFromUserLibraryAsync(int userId, int gameId)
        {
            var user = await _userRepository.GetByIdWithRoleAndGamesAsync(userId);
            if (user == null)
                return false;

            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
                return false;

            try
            {
                user.RemoveGameFromLibrary(game);
                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                // Game is already in the library
                return false;
            }
        }

        // Auxiliar method to map User to UserDTO
        private UserDTO MapUserToDto(User user)
        {
            if (user == null)
                return null;

            var userDto = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name,
                RemoveFlag = user.RemoveFlag,
                Games = user.GameLibrary
                    .Select(gl => new GameDTO
                    {
                        Id = gl.Game.Id,
                        Name = gl.Game.Name,
                        Description = gl.Game.Description,
                        OriginalPrice = gl.Game.Price,
                        Discount = gl.Game.Discount,
                        Price = gl.Game.CalculateDiscountedPrice(gl.Game.Discount),
                        IsOnSale = gl.Game.IsOnSale
                    })
                    .ToList()
            };

            return userDto;
        }
    }
}