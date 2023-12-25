using Composition.Models;
using Composition.ViewModels;
namespace Composition.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Creates Product without it's relationships
        /// </summary>
        /// <param name="viewModel">nullable productviewmodel</param>
        /// <returns>false/true</returns>
        public Task<bool> Create(AddProductViewModel? viewModel);
        /// <summary>
        /// Deleting product by it's id if it's exist (cascade)
        /// </summary>
        /// <param name="id">nullable productid guid</param>
        /// <returns>false/true</returns>
        public Task<bool> DeleteById(Guid? productId);
        /// <summary>
        /// Get current product by it's id
        /// </summary>
        /// <param name="id">nullable productid guid</param>
        /// <returns>product/null</returns>
        public Task<Product?> GetById(Guid? productId);
        /// <summary>
        /// Get all products (moderator only)
        /// </summary>
        /// <returns>All products</returns>
        public Task<List<Product>?> GetAll();
        /// <summary>
        /// Update of this product
        /// </summary>
        /// <param name="updatedProduct">nullable updated product</param>
        /// <returns>false/true</returns>
        public Task<bool> Update(Product? updatedProduct);
    }
}
