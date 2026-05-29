using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebCongNghe.Models;
using WebCongNghe.Repositories;

namespace WebCongNghe.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWebHostEnvironment _environment;

    public ProductController(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IWebHostEnvironment environment)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _environment = environment;
    }

    public async Task<IActionResult> Index(
        int? categoryId,
        string[]? brands,
        string? cpu,
        string? ram,
        decimal? minPrice,
        decimal? maxPrice,
        string sort = "newest",
        int page = 1)
    {
        const int pageSize = 6;

        var products = (await _productRepository.GetAllAsync()).ToList();
        var categories = (await _categoryRepository.GetAllAsync()).ToList();
        var selectedBrands = (brands ?? []).Where(static brand => !string.IsNullOrWhiteSpace(brand)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var projectedProducts = products
            .Select(ProjectCatalogCard)
            .ToList();

        var filteredProducts = projectedProducts.AsEnumerable();

        if (categoryId.HasValue)
        {
            var categoryName = categories.FirstOrDefault(category => category.Id == categoryId.Value)?.Name;
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                filteredProducts = filteredProducts.Where(product =>
                    string.Equals(product.CategoryName, categoryName, StringComparison.OrdinalIgnoreCase));
            }
        }

        if (selectedBrands.Count > 0)
        {
            filteredProducts = filteredProducts.Where(product =>
                selectedBrands.Contains(product.Brand, StringComparer.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(cpu))
        {
            filteredProducts = filteredProducts.Where(product =>
                string.Equals(product.Cpu, cpu, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(ram))
        {
            filteredProducts = filteredProducts.Where(product =>
                string.Equals(product.Ram, ram, StringComparison.OrdinalIgnoreCase));
        }

        if (minPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(product => product.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            filteredProducts = filteredProducts.Where(product => product.Price <= maxPrice.Value);
        }

        filteredProducts = sort switch
        {
            "price-asc" => filteredProducts.OrderBy(product => product.Price),
            "price-desc" => filteredProducts.OrderByDescending(product => product.Price),
            "rating" => filteredProducts.OrderByDescending(product => product.Rating).ThenBy(product => product.Name),
            "name" => filteredProducts.OrderBy(product => product.Name),
            _ => filteredProducts.OrderByDescending(product => product.Id)
        };

        var totalItems = filteredProducts.Count();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
        var currentPage = Math.Clamp(page, 1, totalPages);
        var pagedProducts = filteredProducts
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var currentCategoryName = categoryId.HasValue
            ? categories.FirstOrDefault(category => category.Id == categoryId.Value)?.Name
            : null;

        var categoryOptions = categories
            .Select(category => new CatalogFilterOptionViewModel
            {
                Label = category.Name,
                Value = category.Id.ToString(),
                Count = projectedProducts.Count(product => string.Equals(product.CategoryName, category.Name, StringComparison.OrdinalIgnoreCase)),
                IsSelected = categoryId == category.Id
            })
            .ToList();

        var brandOptions = projectedProducts
            .GroupBy(product => product.Brand)
            .OrderBy(group => group.Key)
            .Select(group => new CatalogFilterOptionViewModel
            {
                Label = group.Key,
                Value = group.Key,
                Count = group.Count(),
                IsSelected = selectedBrands.Contains(group.Key, StringComparer.OrdinalIgnoreCase)
            })
            .ToList();

        var viewModel = new ProductCatalogViewModel
        {
            Products = pagedProducts,
            Categories = categoryOptions,
            Brands = brandOptions,
            CpuOptions = projectedProducts.Select(product => product.Cpu).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(value => value).ToList(),
            RamOptions = projectedProducts.Select(product => product.Ram).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(value => value).ToList(),
            PageTitle = currentCategoryName is null ? "Laptop & thiet bi cong nghe" : $"{currentCategoryName} & thiet bi cong nghe",
            PageDescription = "Khám phá bộ sưu tập laptop cao cấp với hiệu năng đột phá. Cam kết chính hãng 100%, bảo hành tận nơi và hỗ trợ kỹ thuật chuyên sâu từ đội ngũ WebDienTu.",
            CurrentCategoryName = currentCategoryName,
            SelectedCategoryId = categoryId,
            SelectedBrands = selectedBrands,
            SelectedCpu = cpu,
            SelectedRam = ram,
            SelectedMinPrice = minPrice,
            SelectedMaxPrice = maxPrice,
            Sort = sort,
            Page = currentPage,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            StartItem = totalItems == 0 ? 0 : ((currentPage - 1) * pageSize) + 1,
            EndItem = totalItems == 0 ? 0 : Math.Min(currentPage * pageSize, totalItems)
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Add(int? categoryId, string? returnUrl = null)
    {
        await LoadCategoriesAsync(categoryId);
        ViewBag.ReturnUrl = returnUrl;
        return View(new Product
        {
            CategoryId = categoryId ?? 0
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Product product, IFormFile? imageFile, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(product.CategoryId);
            ViewBag.ReturnUrl = returnUrl;
            return View(product);
        }

        if (imageFile is not null)
        {
            product.ImageUrl = await SaveImageAsync(imageFile);
        }

        await _productRepository.AddAsync(product);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Display(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    public async Task<IActionResult> Update(int id, string? returnUrl = null)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        await LoadCategoriesAsync(product.CategoryId);
        ViewBag.ReturnUrl = returnUrl;
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, Product product, IFormFile? imageFile, string? returnUrl = null)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            product.ImageUrl = existingProduct.ImageUrl;
            await LoadCategoriesAsync(product.CategoryId);
            ViewBag.ReturnUrl = returnUrl;
            return View(product);
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Description = product.Description;
        existingProduct.CategoryId = product.CategoryId;

        if (imageFile is not null)
        {
            existingProduct.ImageUrl = await SaveImageAsync(imageFile);
        }

        await _productRepository.UpdateAsync(existingProduct);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, string? returnUrl = null)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        ViewBag.ReturnUrl = returnUrl;
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, string? returnUrl = null)
    {
        await _productRepository.DeleteAsync(id);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(int? selectedCategoryId = null)
    {
        var categories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
    }

    private async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        var imageFolder = Path.Combine(_environment.WebRootPath, "images");
        Directory.CreateDirectory(imageFolder);

        var extension = Path.GetExtension(imageFile.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(imageFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return $"/images/{fileName}";
    }

    private static ProductCatalogCardViewModel ProjectCatalogCard(Product product)
    {
        var brand = DetectBrand(product.Name);
        var priceFactor = brand switch
        {
            "Apple" => 1.12m,
            "ASUS" => 1.08m,
            "Dell" => 1.06m,
            _ => 1.04m
        };

        return new ProductCatalogCardViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = BuildCatalogDescription(product),
            Price = product.Price,
            OriginalPrice = Math.Round(product.Price * priceFactor, 0),
            ImageUrl = product.ImageUrl,
            CategoryName = product.Category?.Name ?? "Sản phẩm",
            Brand = brand,
            Cpu = DetectCpu(product.Name),
            Ram = DetectRam(product.Name),
            Rating = DetectRating(product.Name),
            ReviewCount = DetectReviewCount(product.Name),
            BadgeText = DetectBadgeText(product.Name),
            BadgeTone = DetectBadgeTone(product.Name)
        };
    }

    private static string BuildCatalogDescription(Product product)
    {
        var cpu = DetectCpu(product.Name);
        var ram = DetectRam(product.Name);
        return $"{cpu}, {ram}, bao hanh chinh hang, san sang cho hoc tap, cong viec va sang tao.";
    }

    private static string DetectBrand(string name)
    {
        if (name.Contains("MacBook", StringComparison.OrdinalIgnoreCase) || name.Contains("iPhone", StringComparison.OrdinalIgnoreCase))
        {
            return "Apple";
        }

        if (name.Contains("Dell", StringComparison.OrdinalIgnoreCase))
        {
            return "Dell";
        }

        if (name.Contains("ASUS", StringComparison.OrdinalIgnoreCase) || name.Contains("ROG", StringComparison.OrdinalIgnoreCase))
        {
            return "ASUS";
        }

        if (name.Contains("Lenovo", StringComparison.OrdinalIgnoreCase) || name.Contains("ThinkPad", StringComparison.OrdinalIgnoreCase))
        {
            return "Lenovo";
        }

        return "Khac";
    }

    private static string DetectCpu(string name)
    {
        if (name.Contains("MacBook", StringComparison.OrdinalIgnoreCase) || name.Contains("iPhone", StringComparison.OrdinalIgnoreCase))
        {
            return "Apple M3";
        }

        if (name.Contains("ASUS", StringComparison.OrdinalIgnoreCase) || name.Contains("ROG", StringComparison.OrdinalIgnoreCase))
        {
            return "Ryzen 9";
        }

        if (name.Contains("Dell", StringComparison.OrdinalIgnoreCase) || name.Contains("Lenovo", StringComparison.OrdinalIgnoreCase) || name.Contains("ThinkPad", StringComparison.OrdinalIgnoreCase))
        {
            return "Intel Core i7";
        }

        return "CPU khac";
    }

    private static string DetectRam(string name)
    {
        if (name.Contains("MacBook", StringComparison.OrdinalIgnoreCase))
        {
            return "36GB";
        }

        if (name.Contains("ASUS", StringComparison.OrdinalIgnoreCase) || name.Contains("ROG", StringComparison.OrdinalIgnoreCase))
        {
            return "32GB";
        }

        if (name.Contains("Dell", StringComparison.OrdinalIgnoreCase) || name.Contains("Lenovo", StringComparison.OrdinalIgnoreCase) || name.Contains("ThinkPad", StringComparison.OrdinalIgnoreCase))
        {
            return "16GB";
        }

        return "8GB";
    }

    private static decimal DetectRating(string name) =>
        name switch
        {
            var value when value.Contains("MacBook", StringComparison.OrdinalIgnoreCase) => 4.9m,
            var value when value.Contains("Dell", StringComparison.OrdinalIgnoreCase) => 4.8m,
            var value when value.Contains("ASUS", StringComparison.OrdinalIgnoreCase) || value.Contains("ROG", StringComparison.OrdinalIgnoreCase) => 4.7m,
            var value when value.Contains("Lenovo", StringComparison.OrdinalIgnoreCase) || value.Contains("ThinkPad", StringComparison.OrdinalIgnoreCase) => 4.9m,
            _ => 4.6m
        };

    private static int DetectReviewCount(string name) =>
        name switch
        {
            var value when value.Contains("MacBook", StringComparison.OrdinalIgnoreCase) => 128,
            var value when value.Contains("Dell", StringComparison.OrdinalIgnoreCase) => 92,
            var value when value.Contains("ASUS", StringComparison.OrdinalIgnoreCase) || value.Contains("ROG", StringComparison.OrdinalIgnoreCase) => 215,
            var value when value.Contains("Lenovo", StringComparison.OrdinalIgnoreCase) || value.Contains("ThinkPad", StringComparison.OrdinalIgnoreCase) => 45,
            _ => 64
        };

    private static string DetectBadgeText(string name) =>
        name switch
        {
            var value when value.Contains("MacBook", StringComparison.OrdinalIgnoreCase) => "MOI",
            var value when value.Contains("Dell", StringComparison.OrdinalIgnoreCase) => "HOT",
            var value when value.Contains("ASUS", StringComparison.OrdinalIgnoreCase) || value.Contains("ROG", StringComparison.OrdinalIgnoreCase) => "GAMING",
            var value when value.Contains("Lenovo", StringComparison.OrdinalIgnoreCase) || value.Contains("ThinkPad", StringComparison.OrdinalIgnoreCase) => "WORK",
            _ => "NEW"
        };

    private static string DetectBadgeTone(string name) =>
        name switch
        {
            var value when value.Contains("ASUS", StringComparison.OrdinalIgnoreCase) || value.Contains("ROG", StringComparison.OrdinalIgnoreCase) => "violet",
            var value when value.Contains("Dell", StringComparison.OrdinalIgnoreCase) => "dark",
            var value when value.Contains("Lenovo", StringComparison.OrdinalIgnoreCase) || value.Contains("ThinkPad", StringComparison.OrdinalIgnoreCase) => "slate",
            _ => "orange"
        };
}
