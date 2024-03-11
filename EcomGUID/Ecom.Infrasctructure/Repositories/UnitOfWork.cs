using Ecom.Core.Interfaces;
using Ecom.Infrasctructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrasctructure.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
     
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository {  get; }
        public IProfileRepository ProfileRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository=new CategoryRepository(_context);
            ProductRepository=new ProductRepository(_context);
            ProfileRepository = new ProfileRepository(_context);
        }
    }
}
