using FCG.Core.Entity;
using FCG.Core.Interfaces;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repository
{
    public class GameRepository : EFRepository<Game>, IGameRepository
    {
        public GameRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Game> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<Game> GetAllWithUsers()
        {
            return _dbSet
                .Include(g => g.UserGameLibraries)
                .ThenInclude(ugl => ugl.User)
                .ToList();
        }

        public async Task<IEnumerable<Game>> GetAllWithUsersAsync()
        {
            return await _dbSet
                .Include(g => g.UserGameLibraries)
                .ThenInclude(ugl => ugl.User)
                .ToListAsync();
        }

        public Game GetByIdWithUsers(int id)
        {
            return _dbSet
                .Include(g => g.UserGameLibraries)
                .ThenInclude(ugl => ugl.User)
                .FirstOrDefault(g => g.Id == id);
        }

        public async Task<Game> GetByIdWithUsersAsync(int id)
        {
            return await _dbSet
                .Include(g => g.UserGameLibraries)
                .ThenInclude(ugl => ugl.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public IEnumerable<Game> GetGamesOnSale()
        {
            return _dbSet
                .Where(g => g.Discount > 0)
                .ToList();
        }

        public async Task<IEnumerable<Game>> GetGamesOnSaleAsync()
        {
            return await _dbSet
                .Where(g => g.Discount > 0)
                .ToListAsync();
        }
    }
}
