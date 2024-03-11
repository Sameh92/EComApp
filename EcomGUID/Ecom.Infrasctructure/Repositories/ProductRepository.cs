using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrasctructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrasctructure.Repositories
{
    public class ProductRepository:GenericRepositoryForInteger<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
    }
}
