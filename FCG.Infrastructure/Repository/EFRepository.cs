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
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public EFRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void UpdateData(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteData(int id)
        {
            _dbSet.Remove(GetById(id));
            _context.SaveChanges();
        }
        
        public void InsertData(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual IEnumerable<T> GetAll() => _dbSet.ToList();

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual T GetById(int id) => _dbSet.FirstOrDefault(e => e.Id == id);

        public virtual async Task<T> GetByIdAsync(int id) => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).ToList();

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();

        public virtual void Add(T entity) => _dbSet.Add(entity);

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public virtual void Update(T entity) => _dbSet.Update(entity);

        public virtual void Remove(T entity) => _dbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public virtual bool Any(Expression<Func<T, bool>> predicate) => _dbSet.Any(predicate);

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);
    }
}
