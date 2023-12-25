using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Composition.Database.Methods
{
    public class CategoryMethods : ICategoryRepository
    {
        private readonly AppDBContext appDBContext;
        public CategoryMethods(AppDBContext _appDBContext)
        {
            appDBContext = _appDBContext;
        }
        public async Task<bool> Create(AddCategoryViewModel? viewModel)
        {
            if (viewModel == null)
                return false;
            Category category = new Category() { Name = viewModel.Name, RussianName = viewModel.RussianName };
            await appDBContext.Categories.AddAsync(category);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteById(Guid? id)
        {
            if (id == null)
                return false;
            Category? category = await appDBContext.Categories.FindAsync(id);
            if (category == null)
                return false;
            appDBContext.Categories.Remove(category);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>?> GetAll()
        {
            List<Category> categories = await appDBContext.Categories
                .Include(a => a.ProductCategories)
                .ToListAsync();
            return categories;
        }

        public async Task<Category?> GetById(Guid? categoryId)
        {
            if (categoryId == null || categoryId == Guid.Empty)
                return null;
            Category? category = await appDBContext.Categories.FindAsync(categoryId);
            return category == null ? null : category;
        }

        public async Task<bool> Update(Category? updatedCategory)
        {
            if (updatedCategory == null)
                return false;
            Category? category = await GetById(updatedCategory.Id);
            if (category == null)
                return false;
            category.Name = updatedCategory.Name;
            category.RussianName = updatedCategory.RussianName;
            await appDBContext.SaveChangesAsync();
            return true;
        }
    }
}
