﻿using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Helper.Dtos;
using Ecom.Helper.RequestHelper;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductReposiotry:GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context; 
        private readonly IMapper _mapper;
        private readonly IFileProvider _fileProvider;

        public ProductReposiotry(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _fileProvider = fileProvider;
           
        }
        public async Task<ReturnProductDto> GetAllAsync(ProductParams productParams)
        {
            var result_ = new ReturnProductDto();
            var query = await _context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            //search by name
            if (!string.IsNullOrEmpty(productParams.Search))
                query = query.Where(x => x.Name.ToLower().Contains(productParams.Search)).ToList();
            //filtring by categoryId
            if (productParams.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == productParams.CategoryId.Value).ToList();
            //sorting
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAsc" => query.OrderBy(x => x.Price).ToList(),
                    "PriceDesc" => query.OrderByDescending(x => x.Price).ToList(),
                    _ => query.OrderBy(x => x.Name).ToList(),
                };
            }
            result_.TotalItems = query.Count;
            //paging          
            query = query.Skip((productParams.PageSize) * (productParams.PageNumber - 1)).Take(productParams.PageSize).ToList();
            result_.ProductDtos = _mapper.Map<List<ProductDto>>(query);
            return result_;
        }

        public async Task<bool> AddAsync(CreateProductDto dto)
        {
            var src = "";
            if (dto.Image is not null)
            {
                var root = "/images/products";
                var productName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                if(!Directory.Exists("wwwroot"+root))
                {
                    Directory.CreateDirectory("wwwroot"+root);
                }
                src = root + productName;
                var picInfo = _fileProvider.GetFileInfo(src);
                var rootPath = picInfo.PhysicalPath;
                using (var fileStreeam = new FileStream(rootPath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(fileStreeam);
                }
        
            }
            //Create New Product
            var res = _mapper.Map<Product>(dto);
            res.ProductPicture = src;
            await _context.Products.AddAsync(res);
            await _context.SaveChangesAsync();
            return true;
           
        }
        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {  
            var currentProduct=await _context.Products.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
            if(currentProduct is not null)
            {
                var src = "";
                if (dto.Image is not null)
                {
                    var root = "/images/products";
                    var productName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                    if (!Directory.Exists("wwwroot" + root))
                    {
                        Directory.CreateDirectory("wwwroot" + root);
                    }
                    src = root + productName;
                    var picInfo = _fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;
                    using (var fileStreeam = new FileStream(rootPath, FileMode.Create))
                    {
                        await dto.Image.CopyToAsync(fileStreeam);
                    }

                }
                //remove old picture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    //delete old Picture
                    var picInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
                    var rootPath = picInfo.PhysicalPath;
                    File.Delete(rootPath);
                }
                //Update Product
                var res = _mapper.Map<Product>(dto);
                res.ProductPicture = src;
                res.Id = id;
                _context.Products.Update(res);
                await _context.SaveChangesAsync();

               
                return true;
            }
          return false;

        }
        public async Task<bool> DeleteAsyncWithPicture(int id)
        {
            var currentProduct = await _context.Products.FindAsync(id);
            if(currentProduct is not null)
            {
                //remove old picture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    //delete old Picture
                    var picInfo = _fileProvider.GetFileInfo(currentProduct.ProductPicture);
                    var rootPath = picInfo.PhysicalPath;
                    File.Delete(rootPath);
                }
                _context.Products.Remove(currentProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
