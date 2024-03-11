using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    //Note this way it will caouse error in GetByIdAsync in x.Id=> public class GenericRepository<T> : IGenericRepository<T> where T:class
    public class GenericRepository<T> : IGenericRepository<T> where T:BaseEntity<int>
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var enity= await _context.Set<T>().FindAsync(id);
            if (enity is not null)
            {
                _context.Set<T>().Remove(enity);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<T> GetAll()=>_context.Set<T>().AsNoTracking().ToList();


        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().AsNoTracking().ToListAsync();
       
        // with Include eager loading
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
          var query=_context.Set<T>().AsQueryable();
          var lenghtInculde=includes.Length;
            // apply include
            foreach (var item in includes)
            {
                query=query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id) => await _context.Set<T>().FindAsync(id);
        

        // with Include eager loading
        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().Where(x=>x.Id==id);

            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            //Note return await ((DbSet<T>)query).FindAsync(id);          
            return await query.FirstOrDefaultAsync();
        }
        public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();
        public async Task UpdateAsync(int id, T entity)
        {
            var existingEnity= await _context.Set<T>().FindAsync(id);
            if(existingEnity is not null)
            {
                _context.Update(existingEnity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
