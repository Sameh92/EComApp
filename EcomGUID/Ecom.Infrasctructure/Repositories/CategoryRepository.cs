using Ecom.Core.Entities;
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
    public class CategoryRepository:GenericRepositoryForInteger<Category>,ICategoryRepository 
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
