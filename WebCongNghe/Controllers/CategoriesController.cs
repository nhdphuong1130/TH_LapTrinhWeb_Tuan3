using Microsoft.AspNetCore.Mvc;
using WebCongNghe.Models;
using WebCongNghe.Repositories;

namespace WebCongNghe.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public CategoriesController(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index(int? selectedCategoryId)
    {
        var categories = (await _categoryRepository.GetAllAsync())
            .OrderBy(category => category.Name)
            .ToList();

        if (categories.Count == 0)
        {
            return View(new CategoryManagementViewModel());
        }

        var resolvedCategory = categories.FirstOrDefault(category => category.Id == selectedCategoryId)
            ?? categories.First();

        var allProducts = (await _productRepository.GetAllAsync()).ToList();
        var selectedProducts = allProducts
            .Where(product => product.CategoryId == resolvedCategory.Id)
            .OrderBy(product => product.Name)
            .ToList();

        var viewModel = new CategoryManagementViewModel
        {
            SelectedCategory = resolvedCategory,
            Products = selectedProducts,
            Categories = categories
                .Select(category => new CategoryManagementCategoryItemViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsSelected = category.Id == resolvedCategory.Id,
                    ProductCount = allProducts.Count(product => product.CategoryId == category.Id)
                })
                .ToList()
        };

        return View(viewModel);
    }

    public IActionResult Add()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryRepository.AddAsync(category);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Display(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    public async Task<IActionResult> Update(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(category);
        }

        await _categoryRepository.UpdateAsync(category);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
