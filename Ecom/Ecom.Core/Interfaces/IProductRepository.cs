
using Ecom.Core.Entities;
using Ecom.Helper.Dtos;
using Ecom.Helper.RequestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(CreateProductDto dto);

        Task<bool> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsyncWithPicture(int id);
        Task<ReturnProductDto> GetAllAsync(ProductParams productParams);
    }
}
