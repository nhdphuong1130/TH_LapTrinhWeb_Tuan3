# Homepage Premium Tech Redesign Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Rebuild the homepage into a premium tech storefront using real catalog data while keeping the existing routes and backend intact.

**Architecture:** Add a dedicated homepage view model, prepare all homepage data in `HomeController`, and replace the current homepage Razor with section-based markup for hero, trust strip, featured categories, flash sale, brand story, and newsletter CTA. Scope all styling under homepage-specific CSS so CRUD and catalog pages remain unaffected.

**Tech Stack:** ASP.NET Core MVC (.NET 8), Razor Views, existing repositories, scoped CSS in `wwwroot/css/site.css`

---

## File Structure

- Create: `M:\WebCongNghe\WebCongNghe\Models\HomeViewModel.cs`
- Modify: `M:\WebCongNghe\WebCongNghe\Controllers\HomeController.cs`
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Home\Index.cshtml`
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`

### Task 1: Add Homepage View Model

**Files:**
- Create: `M:\WebCongNghe\WebCongNghe\Models\HomeViewModel.cs`

- [ ] **Step 1: Create homepage view model types**

Add:

```csharp
namespace WebCongNghe.Models;

public class HomeViewModel
{
    public HomeHeroProductViewModel? FeaturedHeroProduct { get; init; }
    public IReadOnlyList<HomeCategoryCardViewModel> FeaturedCategories { get; init; } = [];
    public IReadOnlyList<HomeProductCardViewModel> FlashSaleProducts { get; init; } = [];
}

public class HomeHeroProductViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = "/images/placeholder-product.png";
    public decimal Price { get; init; }
    public decimal? OriginalPrice { get; init; }
    public string CategoryName { get; init; } = string.Empty;
}

public class HomeCategoryCardViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = "/images/placeholder-product.png";
    public int ProductCount { get; init; }
}

public class HomeProductCardViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = "/images/placeholder-product.png";
    public decimal Price { get; init; }
    public decimal? OriginalPrice { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string BadgeText { get; init; } = string.Empty;
}
```

### Task 2: Load Real Homepage Data In Controller

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\Controllers\HomeController.cs`

- [ ] **Step 1: Inject repositories into `HomeController`**

Add constructor dependencies for `IProductRepository` and `ICategoryRepository`.

- [ ] **Step 2: Replace `Index()` with a data-building action**

Implement:

```csharp
public async Task<IActionResult> Index()
{
    var products = (await _productRepository.GetAllAsync()).ToList();
    var categories = (await _categoryRepository.GetAllAsync()).ToList();

    var heroSource = products
        .Where(product => !string.IsNullOrWhiteSpace(product.ImageUrl))
        .OrderByDescending(product => product.Price)
        .FirstOrDefault();

    var featuredCategories = categories
        .Select(category =>
        {
            var categoryProducts = products.Where(product => product.CategoryId == category.Id).ToList();
            var coverProduct = categoryProducts.FirstOrDefault(product => !string.IsNullOrWhiteSpace(product.ImageUrl))
                ?? categoryProducts.FirstOrDefault();

            return new HomeCategoryCardViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = BuildCategoryDescription(category.Name),
                ImageUrl = coverProduct?.ImageUrl ?? "/images/placeholder-product.png",
                ProductCount = categoryProducts.Count
            };
        })
        .Where(category => category.ProductCount > 0)
        .Take(4)
        .ToList();

    var flashSaleProducts = products
        .Where(product => !string.IsNullOrWhiteSpace(product.ImageUrl))
        .OrderByDescending(product => product.Price)
        .Take(4)
        .Select(product => new HomeProductCardViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = string.IsNullOrWhiteSpace(product.Description) ? "San pham cong nghe noi bat trong catalog hien tai." : product.Description,
            ImageUrl = product.ImageUrl ?? "/images/placeholder-product.png",
            Price = product.Price,
            OriginalPrice = Math.Round(product.Price * 1.08m, 0),
            CategoryName = product.Category?.Name ?? "San pham",
            BadgeText = BuildBadgeText(product.Category?.Name)
        })
        .ToList();

    var viewModel = new HomeViewModel
    {
        FeaturedHeroProduct = heroSource is null ? null : new HomeHeroProductViewModel
        {
            Id = heroSource.Id,
            Name = heroSource.Name,
            Description = string.IsNullOrWhiteSpace(heroSource.Description) ? "San pham noi bat voi hieu nang va hinh anh phu hop cho hero storefront." : heroSource.Description,
            ImageUrl = heroSource.ImageUrl ?? "/images/placeholder-product.png",
            Price = heroSource.Price,
            OriginalPrice = Math.Round(heroSource.Price * 1.08m, 0),
            CategoryName = heroSource.Category?.Name ?? "San pham"
        },
        FeaturedCategories = featuredCategories,
        FlashSaleProducts = flashSaleProducts
    };

    return View(viewModel);
}
```

- [ ] **Step 3: Add small private helpers in `HomeController`**

Add:

```csharp
private static string BuildCategoryDescription(string categoryName) => categoryName switch
{
    "Laptop" => "Hieu nang do hoa, di dong va lam viec cao cap.",
    "Dien thoai" => "Flagship, camera va trai nghiem mobile hien dai.",
    "Phu kien" => "Setup, am thanh va phu kien ho tro moi workspace.",
    _ => "Danh muc cong nghe noi bat trong storefront hien tai."
};

private static string BuildBadgeText(string? categoryName) => categoryName switch
{
    "Laptop" => "Performance",
    "Dien thoai" => "Flagship",
    "Phu kien" => "Setup",
    _ => "Featured"
};
```

### Task 3: Replace Homepage Razor

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Home\Index.cshtml`

- [ ] **Step 1: Change the view to `@model HomeViewModel`**

- [ ] **Step 2: Replace current homepage with these sections**

Render:

- hero with product image, price, CTA to catalog and product detail
- trust strip with 4 short selling points
- featured category grid linking to `/Product?categoryId=...`
- flash sale grid with 4 real products and add-to-cart forms
- brand story split section with 2 supporting images from current hero/category/product images
- newsletter CTA section

### Task 4: Add Homepage CSS

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`

- [ ] **Step 1: Replace current `.home-premium` block with the new premium-tech homepage styles**

Add scoped CSS for:

- `.home-premium-shell`
- `.home-premium-hero`
- `.home-premium-trust`
- `.home-premium-category-grid`
- `.home-premium-flash-grid`
- `.home-premium-story`
- `.home-premium-newsletter`

- [ ] **Step 2: Keep palette aligned with `DESIGN.md`**

Use:

- light background / white cards
- dark slate typography
- orange CTA and price accents
- restrained purple technical accents
- 4px/8px corners

### Task 5: Verify Homepage

**Files:**
- Modify: none

- [ ] **Step 1: Stop any running app if needed**

```powershell
Get-Process WebCongNghe -ErrorAction SilentlyContinue | Stop-Process -Force
```

- [ ] **Step 2: Build**

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build succeeds.

- [ ] **Step 3: Start the app**

```powershell
dotnet run --project M:\WebCongNghe\WebCongNghe\WebCongNghe.csproj --urls http://localhost:5057
```

- [ ] **Step 4: Verify homepage response**

```powershell
Invoke-WebRequest http://localhost:5057/ -UseBasicParsing | Select-Object -ExpandProperty StatusCode
```

Expected: `200`

- [ ] **Step 5: Verify homepage includes key sections**

```powershell
$response = Invoke-WebRequest 'http://localhost:5057/' -UseBasicParsing
$content = $response.Content
[PSCustomObject]@{
  HasHero = $content -match 'home-premium-hero'
  HasCategories = $content -match 'home-premium-category-grid'
  HasFlash = $content -match 'home-premium-flash-grid'
} | ConvertTo-Json -Compress
```

Expected:

```json
{"HasHero":true,"HasCategories":true,"HasFlash":true}
```

