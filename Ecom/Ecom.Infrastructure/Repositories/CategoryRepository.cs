using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class CategoryRepository:GenericRepository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context; // need to check if we remove it what will happen
    
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
      
        }
    }
}
