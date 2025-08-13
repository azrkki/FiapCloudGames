using FCG.Core.Entity;
using FCG.Core.Interfaces;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repository
{
    public class RoleRepository : EFRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Role> GetAllWithUsers()
        {
            return _dbSet
                .Include(r => r.Users)
                .ToList();
        }

        public async Task<IEnumerable<Role>> GetAllWithUsersAsync()
        {
            return await _dbSet
                .Include(r => r.Users)
                .ToListAsync();
        }

        public Role GetByIdWithUsers(int id)
        {
            return _dbSet
                .Include(r => r.Users)
                .FirstOrDefault(r => r.Id == id);
        }

        public async Task<Role> GetByIdWithUsersAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Role GetByName(string name)
        {
            return _dbSet
                .FirstOrDefault(r => r.Name == name);
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
