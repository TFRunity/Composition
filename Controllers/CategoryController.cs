using Microsoft.AspNetCore.Mvc;
using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Composition.Controllers
{
    [Authorize(Policy = "Manager")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryMethods;
        private readonly IRelationshipProductCategory relationshipMethods;
        public CategoryController(ICategoryRepository categoryRepository, IRelationshipProductCategory relationshipProductCategory)
        {
            categoryMethods = categoryRepository;
            relationshipMethods = relationshipProductCategory;
        }

        public async Task<IActionResult> ListCategory()
        {
            List<Category>? categories = await categoryMethods.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            AddCategoryViewModel viewModel = new AddCategoryViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(AddCategoryViewModel viewModel)
        {
            bool result = await categoryMethods.Create(viewModel);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось создать категорию." });
            return RedirectToAction(nameof(ListCategory));
        }
        
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            Category? categoryToDelete = await categoryMethods.GetById(categoryId);
            if (categoryToDelete == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Вы пытаетесь удалить несуществующую категорию." });
            if (categoryToDelete.ProductCategories != null)
            {
                foreach (ProductCategory productCategory in categoryToDelete.ProductCategories)
                {
                    bool res = await relationshipMethods.DeleteRelationship(productCategory.ProductId, categoryId);
                    if (res == false)
                        return RedirectToAction("ErrorOccupied", "Home", new { message = "Возникла проблема с удалением какой-то связи категории и продукта." });
                }
            }
            bool result = await categoryMethods.DeleteById(categoryId);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось удалить категорию." });
            return RedirectToAction(nameof(ListCategory));
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(Guid id)
        {
            Category? category = await categoryMethods.GetById(id);
            if (category == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Такой категории не существует." });
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            bool result = await categoryMethods.Update(category);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось обновить категорию." });
            return RedirectToAction(nameof(ListCategory));
        }
    }
}
