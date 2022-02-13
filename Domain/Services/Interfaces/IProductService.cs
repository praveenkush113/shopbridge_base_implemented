using Shopbridge_base.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.Domain.Services.Interfaces
{
    public interface IProductService
    {
        IQueryable<Product> GetAllProduct();
        IEnumerable<Product> GetAllProductList();
        Task<Product> GetProductById(int id);
        Task InsertAndUpdateProduct(Product product, int id = 0);
        Task<bool> DeleteProduct(int id, bool isPermanentDelete);

    }
}
