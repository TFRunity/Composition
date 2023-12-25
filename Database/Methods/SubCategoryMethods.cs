using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;

namespace Composition.Database.Methods
{
    public class SubCategoryMethods : ISubCategoryRepository
    {
        private readonly IProductRepository productMethods;
        private readonly AppDBContext appDBContext;
        public SubCategoryMethods(IProductRepository _productRepository, AppDBContext _appDBContext)
        {
            productMethods = _productRepository;
            appDBContext = _appDBContext;
        }
        public async Task<bool> Create(SubCategoryViewModel? viewModel)
        {
            if (viewModel == null)
                return false;
            if (viewModel.ProductId == Guid.Empty || viewModel.ProductId == null)
                return false;
            bool result = await CreateSubCategory(viewModel);
            if (result == false)
                return false;
            return true;
        }
        private async Task<bool> CreateSubCategory(SubCategoryViewModel viewModel)
        {
            Product? product = await productMethods.GetById(viewModel.ProductId);
            if (product == null)
                return false;
            if ( product.SubCategories != null && product.SubCategories.Count() == 3)
                return false;
            if (product.SubCategories == null || product.SubCategories.Count() == 0)
            {
                SubCategory subCategory = new SubCategory()
                {
                    Case = "First",
                    Price = viewModel.Price,
                    Feature = viewModel.Feature,
                    Url = viewModel.Url
                };
                await appDBContext.SubCategories.AddAsync(subCategory);
                List<SubCategory> subCategories = new List<SubCategory> { subCategory };
                product.SubCategories = subCategories;
                await appDBContext.SaveChangesAsync();
                return true;
            }
            else
            {
                string _case;
                int lenght = product.SubCategories.Count();
                if (lenght == 1)
                    _case = "Second";
                else
                    _case = "Third";
                SubCategory subCategory = new SubCategory()
                {
                    Case = _case,
                    Feature = viewModel.Feature,
                    Price = viewModel.Price,
                    Url = viewModel.Url
                };
                await appDBContext.SubCategories.AddAsync(subCategory);
                product.SubCategories.Add(subCategory);
                await appDBContext.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<SubCategory>?> GetById(Guid? productId)
        {
            Product? product = await productMethods.GetById(productId);
            if (product == null)
                return null;
            if (product.SubCategories == null)
                return null;
            List<SubCategory> subCategories = product.SubCategories;
            return subCategories;
        }

        public async Task<SubCategory?> GetCurrent(Guid? subCategoryId)
        {
            if (subCategoryId == null || subCategoryId == Guid.Empty)
                return null;
            SubCategory? subCategory = await appDBContext.SubCategories.FindAsync(subCategoryId);
            return subCategory == null ? null : subCategory;
        }

        public async Task<bool> Update(SubCategoryViewModel? updatedViewModel)
        {
            if (updatedViewModel == null)
                return false;
            SubCategory? subCategory = await GetCurrent(updatedViewModel.SubCategoryId);
            if (subCategory == null)
                return false;
            subCategory.Url = updatedViewModel.Url;
            subCategory.Price = updatedViewModel.Price;
            subCategory.Feature = updatedViewModel.Feature;
            await appDBContext.SaveChangesAsync();
            return true;
        }
    }
}
