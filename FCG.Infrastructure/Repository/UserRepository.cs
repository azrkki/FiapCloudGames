using FCG.Core.Entity;
using FCG.Core.Interfaces;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repository
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        
        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public override IEnumerable<User> GetAll()
        {
            _logger.LogDebug("UserRepository.GetAll called");
            try
            {
                var users = _dbSet
                    .Include(u => u.Role)
                    .Include(u => u.GameLibrary)
                        .ThenInclude(gl => gl.Game)
                    .ToList();
                _logger.LogDebug("Successfully retrieved {Count} users from database", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users from database");
                throw;
            }
        }

        public User GetByEmail(string email)
        {
            _logger.LogDebug("UserRepository.GetByEmail called with email: {Email}", email);
            try
            {
                var user = _dbSet
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Email == email);
                
                if (user != null)
                {
                    _logger.LogDebug("User found with email: {Email} (ID: {UserId})", email, user.Id);
                }
                else
                {
                    _logger.LogDebug("No user found with email: {Email}", email);
                }
                
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
                throw;
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public IEnumerable<User> GetAllWithRolesAndGames()
        {
            return _dbSet
                .Include(u => u.Role)
                .Include(u => u.GameLibrary)
                    .ThenInclude(gl => gl.Game)
                .ToList();
        }

        public async Task<IEnumerable<User>> GetAllWithRolesAndGamesAsync()
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.GameLibrary)
                    .ThenInclude(gl => gl.Game)
                .ToListAsync();
        }

        public User GetByIdWithRoleAndGames(int id)
        {
            return _dbSet
                .Include(u => u.Role)
                .Include(u => u.GameLibrary)
                    .ThenInclude(gl => gl.Game)
                .FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> GetByIdWithRoleAndGamesAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.GameLibrary)
                    .ThenInclude(gl => gl.Game)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
