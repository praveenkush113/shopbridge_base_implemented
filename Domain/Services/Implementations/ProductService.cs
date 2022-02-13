using Microsoft.Extensions.Logging;

using Shopbridge_base.Data.Repository;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IRepository<Product> _productRepository;

        public ProductService(ILogger<ProductService> logger, IRepository<Product> productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        // Get all Product
        public IQueryable<Product> GetAllProduct()
        {
            try
            {
                return _productRepository.Table.Where(s=>!s.IsDeleted).OrderByDescending(o => o.Product_Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ProductService|GetAllProduct{ex.GetBaseException()}", "Get");
                throw new Exception("something wrong for get all products");
            }
        }
        // Get all Product
        public IEnumerable<Product> GetAllProductList()
        {
            try
            {
                return _productRepository.GetAll().OrderByDescending(o => o.Product_Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ProductService|GetAllProduct{ex.GetBaseException()}", "Get");
                throw new Exception("something wrong for get all products");
            }
        }
        //get product by Id
        public async Task<Product> GetProductById(int id)
        {
            try
            {
                return  _productRepository.Table.First(s=>s.Product_Id==id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ProductService|GetProductById{ex.GetBaseException()}", "Get by id"+ id);
                throw new Exception("something wrong for get  product");
            }
        }
        public async Task InsertAndUpdateProduct(Product product , int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    //update product
                    var productEntity =  _productRepository.Table.First(s=>s.Product_Id==id);
                    productEntity.UpdatedAt = DateTime.UtcNow;
                    productEntity.Price = product.Price;
                    productEntity.Name = product.Name;
                    productEntity.Description = product.Description;
                    await _productRepository.Update(productEntity);
                }
                else
                {
                    //insert product
                    product.CreatedAt = DateTime.UtcNow;
                    await _productRepository.Insert(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ProductService|InsertAndUpdateProduct{ex.GetBaseException()}", "insert/update  product"  );
                throw new Exception("something wrong for insert/update  product");
            }
        }
        public async Task<bool> DeleteProduct(int id,bool isPermanentDelete)
        {
            try
            {
               var product= await GetProductById(id);
                if (isPermanentDelete)
                {
                    // hard delete
                    await _productRepository.Delete(product);
                }
                else
                {
                    //soft delete
                    product.IsDeleted = !isPermanentDelete;
                    await _productRepository.Update(product);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ProductService|DeleteProduct{ex.GetBaseException()}", "delete id" + id);
                throw new Exception("something wrong for delete  product");
            }
        }
    }
}
