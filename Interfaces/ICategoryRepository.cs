using Composition.Models;
using Composition.ViewModels;
namespace Composition.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Creates category if params is ok
        /// </summary>
        /// <param name="viewModel"> nullable categoryviewmodel</param>
        /// <returns>false/true</returns>
        public Task<bool> Create(AddCategoryViewModel? viewModel);
        /// <summary>
        /// Deleting category if param is ok
        /// </summary>
        /// <param name="id"> nullable Guid id</param>
        /// <returns>false/true</returns>
        public Task<bool> DeleteById(Guid? categoryId);
        /// <summary>
        /// Returns category if it's exist
        /// </summary>
        /// <param name="id"> nullable Guid id</param>
        /// <returns>category/null</returns>
        public Task<Category?> GetById(Guid? categoryId);
        /// <summary>
        /// All categories
        /// </summary>
        /// <returns>Returns all Categories with all products in them</returns>
        public Task<List<Category>?> GetAll();
        /// <summary>
        /// Update of this category
        /// </summary>
        /// <param name="updatedCategory">nullable category</param>
        /// <returns>false/true</returns>
        public Task<bool> Update(Category? updatedCategory);
    }
}
