﻿using Ecom.Core.Interfaces;
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
    public class GenercRepository<T> : IGenereicRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenercRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll() => _context.Set<T>().AsNoTracking().ToList();


        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();


        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includs)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var item in includs)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }
    }

}
