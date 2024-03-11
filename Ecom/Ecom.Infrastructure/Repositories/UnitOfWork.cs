using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {     
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IBasketRepository BasketRepository {  get;  }
        private readonly IConnectionMultiplexer _redis;

        public UnitOfWork(ApplicationDbContext context , IFileProvider fileProvider, IMapper mapper,IConnectionMultiplexer redis)     
        {
            _redis=redis;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductReposiotry(context,fileProvider,mapper);
            BasketRepository = new BasketRepository(_redis);
           
        }

    }
}
