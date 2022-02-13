using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopbridge_base.Data;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Domain.Services.Interfaces;

namespace Shopbridge_base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService _productService,  ILogger<ProductsController> logger)
        {
            this.productService = _productService;
            _logger = logger;
        }

       
        [HttpGet]   
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            try
            {
                return productService.GetAllProductList().ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ProductsController|GetProduct{ex.GetBaseException()}", "GetAll");
                throw new Exception("something wrong for get all products");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                if (ProductExists(id))
                { 
                    return await productService.GetProductById(id);

                }
                else
                {
                    throw new Exception("product not found");
                }
                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"ProductsController|GetProduct{ex.GetBaseException()}", "Get");
                throw new Exception("something wrong for get  product");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id,Product product)
        {
            try
            {
                if (ProductExists(id))
                {
                    await productService.InsertAndUpdateProduct(product, id);
                }
                else
                {
                    throw new Exception("product not found");
                }
            return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"ProductsController|PutProduct{ex.GetBaseException()}", "update");
                throw new Exception("something wrong for update products");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                await productService.InsertAndUpdateProduct(product);
                return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"ProductsController|PostProduct{ex.GetBaseException()}", "insert");
                throw new Exception("something wrong for insert products");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id,bool isPermanentDelete)
        {
            try
            {
            if (ProductExists(id))
            { 
                await productService.DeleteProduct(id, isPermanentDelete);
            }
            else
            {
                throw new Exception("product not found");
            }
            return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"ProductsController|GetAllProduct{ex.GetBaseException()}", "Get");
                throw new Exception("something wrong for get all products");
            }
        }

        private bool ProductExists(int id)
        {
            return productService.GetAllProduct().Any(s => s.Product_Id == id);
        }
    }
}
