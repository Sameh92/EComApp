﻿using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrasctructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrasctructure.Repositories
{
    public class GenericRepositoryForGuid<T> :GenercRepository<T>, IGenericRepositoryForGuidId<T> where T : BaseEntity<Guid>
    {
        private readonly ApplicationDbContext _context;
        public GenericRepositoryForGuid(ApplicationDbContext context):base(context)
        { 
         _context = context;
        }

        public async Task DeleteAsync(Guid id)
        {
           var entity= await _context.Set<T>().FindAsync(id);
            if (entity is not null)
            {
              _context.Set<T>().Remove(entity);
              _context.SaveChanges();
            }
        }

        public async Task<T> GetAsync(Guid id) => await _context.Set<T>().FindAsync(id);
      

        public async Task<T> GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().Where(x=>x.Id==id);
            foreach(var item in includes)
            {
                query = query.Include(item);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var existingEnity = await _context.Set<T>().FindAsync(id);
            if (existingEnity is not null)
            {
                _context.Update(existingEnity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
