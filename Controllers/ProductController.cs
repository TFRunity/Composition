using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Composition.Controllers
{
    [Authorize(Policy = "Manager")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productMethods;
        private readonly IRelationshipProductCategory relationshipMethods;
        private readonly ISubCategoryRepository subCategoryMethods;
        public ProductController(IProductRepository productRepository, IRelationshipProductCategory relationshipProductCategory, ISubCategoryRepository _subCategoryMethods)
        {
            productMethods = productRepository;
            relationshipMethods = relationshipProductCategory;
            subCategoryMethods = _subCategoryMethods;

        }

        //
        //Product interaction
        //

        public async Task<IActionResult> ListProduct()
        {
            List<Product>? products = await productMethods.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            AddProductViewModel viewModel = new AddProductViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(AddProductViewModel productViewModel)
        {
            bool result = await productMethods.Create(productViewModel);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось создать объект." });
            return RedirectToAction(nameof(ListProduct));
        }

        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            Product? productToDelete = await productMethods.GetById(productId);
            if (productToDelete != null && productToDelete.ProductCategories != null)
            {
                foreach (ProductCategory relationship in productToDelete.ProductCategories)
                {
                    await relationshipMethods.DeleteRelationship(productId, relationship.CategoryId);
                }
                await productMethods.DeleteById(productId);
                return RedirectToAction(nameof(ListProduct));
            }
            return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось удалить объект." });
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            Product? product = await productMethods.GetById(id);
            if (product == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не существует такого объекта." });
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            bool result = await productMethods.Update(product);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не получилось обновить объект." });
            return RedirectToAction(nameof(ListProduct));
        }

        //
        //Subcategory interaction
        //

        [HttpGet]
        public async Task<IActionResult> AddSubCategory(Guid productId)
        {
            Product? product = await productMethods.GetById(productId);
            if (product == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не существует такого товара." });
            if (product.SubCategories == null)
            {
                SubCategoryViewModel _viewModel = new SubCategoryViewModel()
                {
                    ProductId = productId,
                    _Case = "First"
                };
                return View(_viewModel);
            }
            else
            {
                SubCategoryViewModel _viewModel = new SubCategoryViewModel()
                {
                    ProductId = productId,
                    _Case = "First"
                };
                switch (product.SubCategories.Count())
                {
                    case 0:
                        return View(_viewModel);
                    case 1:
                        _viewModel._Case = "Second";
                        break;
                    case 2:
                        _viewModel._Case = "Third";
                        break;
                    default:
                        return RedirectToAction(nameof(UpdateSubCategory), new { productId = productId });
                }
                return View(_viewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddSubCategory(SubCategoryViewModel viewModel)
        {
            bool result = await subCategoryMethods.Create(viewModel);
            if(result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не удалось добавить подкатегорию." });
            return RedirectToAction(nameof(ListProduct));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSubCategory(Guid productId)
        {
            Product? product = await productMethods.GetById(productId);
            if(product == null)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "2Не существует такого товара." });
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSubCategory(SubCategoryViewModel viewModel)
        {
            bool result = await subCategoryMethods.Update(viewModel);
            if (result == false)
                return RedirectToAction("ErrorOccupied", "Home", new { message = "Не существует такого товара." });
            return RedirectToAction(nameof(UpdateSubCategory), new { productId = viewModel.ProductId });
        }
    }
}
