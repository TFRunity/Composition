using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Composition.Database.Methods
{
    public class ProductMethods : IProductRepository
    {
        private readonly AppDBContext appDBContext;
        public ProductMethods(AppDBContext _appDBContext)
        {
            appDBContext = _appDBContext;
        }
        public async Task<bool> Create(AddProductViewModel? viewModel)
        {
            if (viewModel == null)
                return false;
            Product product = new Product() { Name = viewModel.Name, Description = viewModel.Description, Price = viewModel.Price, URL = viewModel.Url };
            await appDBContext.Products.AddAsync(product);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteById(Guid? productId)
        {
            if (productId == null || productId == Guid.Empty)
                return false;
            Product? product = await appDBContext.Products.FindAsync(productId);
            if (product == null)
                return false;
            appDBContext.Products.Remove(product);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>?> GetAll()
        {
            List<Product> products = await appDBContext.Products
                .Include(a => a.ProductCategories)
                .ToListAsync();
            return products;
        }

        public async Task<Product?> GetById(Guid? productId)
        {
            if (productId == null || productId == Guid.Empty)
                return null;
            Product? product = await appDBContext.Products.FindAsync(productId);
            return product == null ? null : product;
        }

        public async Task<bool> Update(Product? updatedProduct)
        {
            if (updatedProduct == null)
                return false;
            Product? product = await GetById(updatedProduct.Id);
            if (product == null)
                return false;
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.URL = updatedProduct.URL;
            await appDBContext.SaveChangesAsync();
            return true;
        }
    }
}
