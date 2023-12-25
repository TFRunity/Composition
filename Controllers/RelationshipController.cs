using Composition.Database.Methods;
using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Composition.Controllers
{
    public class RelationshipController : Controller
    {
        private readonly IRelationshipProductCategory relationshipMethods;
        private readonly IProductRepository productMethods;
        private readonly ICategoryRepository categoryMethods;
        public RelationshipController(IRelationshipProductCategory relationshipProductCategory, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            relationshipMethods = relationshipProductCategory;
            productMethods = productRepository;
            categoryMethods = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductRelationship(Guid productId)
        {
            Product? product = await productMethods.GetById(productId);
            if (product == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не существует такого объекта." });
            return View(product);
        }
        public async Task<IActionResult> CreateProductRelationships(Guid productId, Guid categoryId)
        {
            bool result = await relationshipMethods.CreateRelationship(productId, categoryId);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось создать связь." });
            return RedirectToAction("ListProduct","Product");
        }
        public async Task<IActionResult> DeleteProductRelationship(Guid productId, Guid categoryId)
        {
            bool result = await relationshipMethods.DeleteRelationship(productId, categoryId);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось удалить связь." });
            return RedirectToAction("ListProduct", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeRelationships(Guid productId)
        {
            Product? product = await productMethods.GetById(productId);
            if(product == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось удалить связь." });
            if(product.ProductCategories == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "У объекта не существует связей." });
            List<ProductCategory> productCategories = product.ProductCategories;
            List<Guid> categoryIds = new List<Guid>();
            List<Category>? categories = await categoryMethods.GetAll();
            if (categories == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Категорий не существует." });
            List<Guid> allcategoryIds = new List<Guid>();
            foreach (ProductCategory pc in productCategories)
            {
                categoryIds.Add(pc.CategoryId);
            }
            foreach (Category category in categories)
            {
                allcategoryIds.Add(category.Id);
            }
            ChangeRelationshipViewModel viewModel = new ChangeRelationshipViewModel()
            {
                ProductId = productId,
                CategoryIds = categoryIds,
                AllCategories = allcategoryIds
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRelationships(ChangeRelationshipViewModel viewModel)
        {
            Product? product = await productMethods.GetById(viewModel.ProductId);
            if (product == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Объект не существует." });
            if (product.ProductCategories == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "У объекта не существует связей." });
            List<Guid> categoryIds = new List<Guid>();
            foreach (ProductCategory pc in product.ProductCategories)
            {
                categoryIds.Add(pc.CategoryId);
            }
            List<Guid> addedCategories = (List<Guid>)viewModel.AllCategories.Except(categoryIds);
            bool result = await relationshipMethods.UpdateRelationship(addedCategories,viewModel.ProductId);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось изменить связи." });
            return RedirectToAction("ListProduct","Product");
        }
    }
}
