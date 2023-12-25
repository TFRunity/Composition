using Composition.Models;
using Composition.ViewModels;
namespace Composition.Interfaces
{
    public interface ISubCategoryRepository
    {
        /// <summary>
        /// Creates a subcategoryviewmodel: if it's first subcategory => adds subcategory else: adds by it'
        /// s case
        /// </summary>
        /// <param name="viewModel"> nullable subcategory viewmodel</param>
        /// <returns>true/false</returns>
        public Task<bool> Create(SubCategoryViewModel? viewModel);
        /// <summary>
        /// Get all sub categories of current product
        /// </summary>
        /// <param name="id">productid nullable guid</param>
        /// <returns>all sub categories of this product</returns>
        public Task<List<SubCategory>?> GetById(Guid? id);
        /// <summary>
        /// Get current (1) sub category of product (1 of 3)
        /// </summary>
        /// <param name="id">nullable guid of this sub category</param>
        /// <returns>sub category/null</returns>
        public Task<SubCategory?> GetCurrent(Guid? productid);
        /// <summary>
        /// Updates current 1 sub category
        /// </summary>
        /// <param name="updatedViewModel">nullable viewmodel of changeable sub category</param>
        /// <returns>true/false</returns>
        public Task<bool> Update(SubCategoryViewModel? updatedViewModel);
    }
}
