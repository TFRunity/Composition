using Composition.Interfaces;
using Composition.Models;
using Microsoft.EntityFrameworkCore;

namespace Composition.Database.Methods
{
    public class RelationshipProductCategoryMethods : IRelationshipProductCategory
    {
        private readonly ICategoryRepository categoryMethods;
        private readonly IProductRepository productMethods;
        private readonly AppDBContext appDBContext;
        public RelationshipProductCategoryMethods(ICategoryRepository _categoryMethods, IProductRepository _productMethods, AppDBContext _appDBContext)
        {
            categoryMethods = _categoryMethods;
            productMethods = _productMethods;
            appDBContext = _appDBContext;
        }
        public async Task<bool> CreateRelationship(Guid productId, Guid categoryId)
        {
            ProductCategory productCategory = new ProductCategory()
            {
                ProductId = productId,
                CategoryId = categoryId
            };
            await appDBContext.ProductCategories.AddAsync(productCategory);
            Product? product = await productMethods.GetById(productId);
            if (product == null)
                return false;
            product.ProductCategories.Add(productCategory);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRelationship(Guid productId, Guid categoryId)
        {
            Product? product = await productMethods.GetById(productId);
            if(product == null)
                return false;
            ProductCategory? productCategoryToRemove = await appDBContext.ProductCategories.Where(x => x.CategoryId == categoryId).FirstOrDefaultAsync();
            if (productCategoryToRemove == null)
                return false;
            product.ProductCategories.Remove(productCategoryToRemove);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRelationship(List<Guid> categoryIds, Guid productId)
        {
            bool result = await DeleteProductCategoryWithCurrentId(productId);
            if (result == false)
                return false;
            bool res = await UpdateRelationshipsForProduct(productId, categoryIds);
            if (res == false)
                return false;
            return true;
        }
        private async Task<bool> DeleteProductCategoryWithCurrentId(Guid productId)
        {
            List<Category>? allCategories = await categoryMethods.GetAll();
            if (allCategories == null)
                return false;
            foreach (Category category in allCategories)
            {
                if (category.ProductCategories != null)
                {
                    foreach (ProductCategory pc in category.ProductCategories)
                    {
                        if (pc.ProductId == productId)
                            await DeleteRelationship(productId, category.Id);
                    }
                }
                else
                {
                    continue;
                }
            }
            return true;
        }
        private async Task<bool> UpdateRelationshipsForProduct(Guid productId, List<Guid> categoryIds)
        {
            Product? product = await productMethods.GetById(productId);
            if (product == null)
                return false;
            product.ProductCategories.Clear();
            List<ProductCategory> productCategories = new List<ProductCategory>();
            foreach (Guid categoryId in categoryIds)
            {
                Category? category = await categoryMethods.GetById(categoryId);
                if (category == null)
                    continue;
                ProductCategory productCategory = new ProductCategory()
                {
                    ProductId = productId,
                    CategoryId = categoryId
                };
                productCategories.Add(productCategory);
            }
            product.ProductCategories = productCategories;
            await appDBContext.SaveChangesAsync();
            return true;
        }
    }
}
