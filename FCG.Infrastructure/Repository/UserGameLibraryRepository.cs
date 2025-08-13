using FCG.Core.Entity;
using FCG.Core.Interfaces;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repository
{
    public class UserGameLibraryRepository : IUserGameLibraryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<UserGameLibrary> _dbSet;

        public UserGameLibraryRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<UserGameLibrary>();
        }

        public IEnumerable<UserGameLibrary> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<UserGameLibrary>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public UserGameLibrary GetById(int userId, int gameId)
        {
            return _dbSet.FirstOrDefault(ugl => ugl.UserId == userId && ugl.GameId == gameId);
        }

        public async Task<UserGameLibrary> GetByIdAsync(int userId, int gameId)
        {
            return await _dbSet.FirstOrDefaultAsync(ugl => ugl.UserId == userId && ugl.GameId == gameId);
        }

        public IEnumerable<UserGameLibrary> Find(Expression<Func<UserGameLibrary, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public async Task<IEnumerable<UserGameLibrary>> FindAsync(Expression<Func<UserGameLibrary, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public void Add(UserGameLibrary entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(UserGameLibrary entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<UserGameLibrary> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<UserGameLibrary> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Remove(UserGameLibrary entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<UserGameLibrary> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(UserGameLibrary entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<UserGameLibrary> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public int Count(Expression<Func<UserGameLibrary, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<UserGameLibrary, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public bool Any(Expression<Func<UserGameLibrary, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<UserGameLibrary, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public IEnumerable<UserGameLibrary> GetAllWithUsersAndGames()
        {
            return _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .ToList();
        }

        public async Task<IEnumerable<UserGameLibrary>> GetAllWithUsersAndGamesAsync()
        {
            return await _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .ToListAsync();
        }

        public UserGameLibrary GetByUserIdAndGameId(int userId, int gameId)
        {
            return _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .FirstOrDefault(ugl => ugl.UserId == userId && ugl.GameId == gameId);
        }

        public async Task<UserGameLibrary> GetByUserIdAndGameIdAsync(int userId, int gameId)
        {
            return await _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .FirstOrDefaultAsync(ugl => ugl.UserId == userId && ugl.GameId == gameId);
        }

        public IEnumerable<UserGameLibrary> GetByUserId(int userId)
        {
            return _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .Where(ugl => ugl.UserId == userId)
                .ToList();
        }

        public async Task<IEnumerable<UserGameLibrary>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .Where(ugl => ugl.UserId == userId)
                .ToListAsync();
        }

        public IEnumerable<UserGameLibrary> GetByGameId(int gameId)
        {
            return _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .Where(ugl => ugl.GameId == gameId)
                .ToList();
        }

        public async Task<IEnumerable<UserGameLibrary>> GetByGameIdAsync(int gameId)
        {
            return await _dbSet
                .Include(ugl => ugl.User)
                .Include(ugl => ugl.Game)
                .Where(ugl => ugl.GameId == gameId)
                .ToListAsync();
        }
    }
}