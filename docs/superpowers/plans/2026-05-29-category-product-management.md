# Category Product Management Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a category-first management screen where selecting a category shows its products and lets the user add, view, edit, and delete products in that category flow.

**Architecture:** Keep the existing `Category -> Product` relationship and reuse `ProductController` for product CRUD. Move category-oriented orchestration into `CategoriesController.Index` with a dedicated view model, then preserve category context across product CRUD using route values and return URLs.

**Tech Stack:** ASP.NET Core MVC (.NET 8), Razor Views, Entity Framework Core, SQL Server, existing repository pattern, scoped CSS in `wwwroot/css/site.css`

---

## File Structure

- Create: `M:\WebCongNghe\WebCongNghe\Models\CategoryManagementViewModel.cs`
  - Typed data for the category master-detail screen.
- Modify: `M:\WebCongNghe\WebCongNghe\Repositories\IProductRepository.cs`
  - Add a repository method for loading products by category.
- Modify: `M:\WebCongNghe\WebCongNghe\Repositories\EFProductRepository.cs`
  - Implement category-filtered product loading.
- Modify: `M:\WebCongNghe\WebCongNghe\Controllers\CategoriesController.cs`
  - Load categories, resolve selected category, and build the management view model.
- Modify: `M:\WebCongNghe\WebCongNghe\Controllers\ProductController.cs`
  - Accept category context when adding and preserve return path on add/update/delete.
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Categories\Index.cshtml`
  - Replace flat category list with a master-detail management screen.
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Product\Add.cshtml`
  - Preserve selected category context when creating a product from category management.
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Product\Update.cshtml`
  - Preserve return path back to category management.
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Product\Delete.cshtml`
  - Preserve return path back to category management.
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`
  - Add scoped styles for the category-product management layout.

### Task 1: Add Category Management Data Shape

**Files:**
- Create: `M:\WebCongNghe\WebCongNghe\Models\CategoryManagementViewModel.cs`
- Modify: `M:\WebCongNghe\WebCongNghe\Repositories\IProductRepository.cs`
- Modify: `M:\WebCongNghe\WebCongNghe\Repositories\EFProductRepository.cs`

- [ ] **Step 1: Create the view model file**

```csharp
using WebCongNghe.Models;

namespace WebCongNghe.Models;

public class CategoryManagementViewModel
{
    public List<CategoryManagementCategoryItemViewModel> Categories { get; init; } = [];
    public Category? SelectedCategory { get; init; }
    public List<Product> Products { get; init; } = [];
    public int TotalCategories => Categories.Count;
    public int SelectedProductCount => Products.Count;
    public bool HasCategories => Categories.Count > 0;
    public bool HasSelectedCategory => SelectedCategory is not null;
}

public class CategoryManagementCategoryItemViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsSelected { get; init; }
    public int ProductCount { get; init; }
}
```

- [ ] **Step 2: Add a repository contract for category-filtered products**

Update `M:\WebCongNghe\WebCongNghe\Repositories\IProductRepository.cs`:

```csharp
using WebCongNghe.Models;

namespace WebCongNghe.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
```

- [ ] **Step 3: Implement category-filtered product loading**

Update `M:\WebCongNghe\WebCongNghe\Repositories\EFProductRepository.cs`:

```csharp
public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
{
    return await _context.Products
        .Include(p => p.Category)
        .AsNoTracking()
        .Where(p => p.CategoryId == categoryId)
        .OrderBy(p => p.Name)
        .ToListAsync();
}
```

- [ ] **Step 4: Build the solution to verify the new contract compiles**

Run:

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build fails in `CategoriesController` because the new view model is not wired yet, or succeeds if no controller references exist yet.

- [ ] **Step 5: Commit**

If `.git` is restored later:

```powershell
git add WebCongNghe\Models\CategoryManagementViewModel.cs WebCongNghe\Repositories\IProductRepository.cs WebCongNghe\Repositories\EFProductRepository.cs
git commit -m "feat: add category management data model"
```

### Task 2: Load Category-Oriented Management Data

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\Controllers\CategoriesController.cs`

- [ ] **Step 1: Replace the current `Index` action with category selection logic**

Update `M:\WebCongNghe\WebCongNghe\Controllers\CategoriesController.cs` constructor and `Index`:

```csharp
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

        var products = (await _productRepository.GetByCategoryIdAsync(resolvedCategory.Id)).ToList();
        var productCounts = categories.ToDictionary(
            category => category.Id,
            category => products.Count(product => product.CategoryId == category.Id));

        if (categories.Count > 1)
        {
            var allProducts = (await _productRepository.GetAllAsync()).ToList();
            productCounts = categories.ToDictionary(
                category => category.Id,
                category => allProducts.Count(product => product.CategoryId == category.Id));
        }

        var viewModel = new CategoryManagementViewModel
        {
            SelectedCategory = resolvedCategory,
            Products = products,
            Categories = categories
                .Select(category => new CategoryManagementCategoryItemViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsSelected = category.Id == resolvedCategory.Id,
                    ProductCount = productCounts[category.Id]
                })
                .ToList()
        };

        return View(viewModel);
    }
}
```

- [ ] **Step 2: Keep the existing category CRUD actions untouched**

Confirm `Add`, `Display`, `Update`, and `Delete` remain in place after the `Index` change. No extra code needed here beyond preserving the existing methods.

- [ ] **Step 3: Build to verify controller wiring**

Run:

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build may still fail because `Views/Categories/Index.cshtml` still expects `IEnumerable<Category>`.

- [ ] **Step 4: Commit**

If `.git` is restored later:

```powershell
git add WebCongNghe\Controllers\CategoriesController.cs
git commit -m "feat: load category management workspace data"
```

### Task 3: Preserve Category Context Across Product CRUD

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\Controllers\ProductController.cs`
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Product\Add.cshtml`
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Product\Update.cshtml`
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Product\Delete.cshtml`

- [ ] **Step 1: Update `Add` GET to accept preselected category and return path**

Replace the current `Add` GET signature and body in `ProductController`:

```csharp
public async Task<IActionResult> Add(int? categoryId, string? returnUrl = null)
{
    await LoadCategoriesAsync(categoryId);
    ViewBag.ReturnUrl = returnUrl;

    return View(new Product
    {
        CategoryId = categoryId ?? 0
    });
}
```

- [ ] **Step 2: Update `Add` POST to preserve return path**

Change the `Add` POST signature and redirect:

```csharp
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

    return RedirectToAction(nameof(Index), new { categoryId = product.CategoryId });
}
```

- [ ] **Step 3: Update `Update` GET and POST to preserve return path**

Change `Update` in `ProductController`:

```csharp
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

    return RedirectToAction(nameof(Index), new { categoryId = existingProduct.CategoryId });
}
```

- [ ] **Step 4: Update `Delete` GET and POST to preserve return path**

Change `Delete` in `ProductController`:

```csharp
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
    var product = await _productRepository.GetByIdAsync(id);
    await _productRepository.DeleteAsync(id);

    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
    {
        return Redirect(returnUrl);
    }

    return RedirectToAction(nameof(Index), new { categoryId = product?.CategoryId });
}
```

- [ ] **Step 5: Add hidden `returnUrl` fields to product forms**

Add this hidden field inside the main `<form>` of:

- `M:\WebCongNghe\WebCongNghe\Views\Product\Add.cshtml`
- `M:\WebCongNghe\WebCongNghe\Views\Product\Update.cshtml`
- `M:\WebCongNghe\WebCongNghe\Views\Product\Delete.cshtml`

```cshtml
<input type="hidden" name="returnUrl" value="@ViewBag.ReturnUrl" />
```

- [ ] **Step 6: Build to verify controller and form signatures**

Run:

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build may still fail until `Categories/Index.cshtml` is updated to the new model type.

- [ ] **Step 7: Commit**

If `.git` is restored later:

```powershell
git add WebCongNghe\Controllers\ProductController.cs WebCongNghe\Views\Product\Add.cshtml WebCongNghe\Views\Product\Update.cshtml WebCongNghe\Views\Product\Delete.cshtml
git commit -m "feat: preserve category management context in product crud"
```

### Task 4: Replace Category Index With Master-Detail Management UI

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Categories\Index.cshtml`
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`

- [ ] **Step 1: Replace the view model and page markup**

Replace `M:\WebCongNghe\WebCongNghe\Views\Categories\Index.cshtml` with:

```cshtml
@model WebCongNghe.Models.CategoryManagementViewModel

@{
    ViewData["Title"] = "Danh muc";
    var selectedCategory = Model.SelectedCategory;
    var returnUrl = selectedCategory is null
        ? Url.Action("Index", "Categories")
        : Url.Action("Index", "Categories", new { selectedCategoryId = selectedCategory.Id });
}

<section class="page-header">
    <div>
        <span class="eyebrow">Category management</span>
        <h1 class="page-title">Quan ly san pham theo tung danh muc trong cung mot man hinh.</h1>
        <p class="page-copy mb-0 mt-3">Chon danh muc o cot trai, xem toan bo san pham thuoc danh muc do va thao tac CRUD ngay trong workspace ben phai.</p>
    </div>
    <div class="page-actions">
        <a asp-action="Add" class="btn btn-electric">Them danh muc</a>
        @if (selectedCategory is not null)
        {
            <a asp-controller="Product" asp-action="Add" asp-route-categoryId="@selectedCategory.Id" asp-route-returnUrl="@returnUrl" class="btn btn-outline-light">Them san pham</a>
        }
    </div>
</section>

@if (!Model.HasCategories)
{
    <section class="empty-state">
        <h2 class="section-title">Chua co danh muc nao.</h2>
        <p class="section-copy mx-auto mt-3 mb-4">Tao danh muc truoc de co the nhom va quan ly san pham theo tung khu vuc rieng.</p>
        <a asp-action="Add" class="btn btn-electric">Tao danh muc</a>
    </section>
}
else
{
    <section class="category-management">
        <aside class="category-management__rail">
            <div class="category-management__panel">
                <div class="category-management__panel-head">
                    <span class="category-pill">Danh muc</span>
                    <strong>@Model.TotalCategories</strong>
                </div>

                <div class="category-management__category-list">
                    @foreach (var category in Model.Categories)
                    {
                        <a asp-action="Index"
                           asp-route-selectedCategoryId="@category.Id"
                           class="category-management__category-item @(category.IsSelected ? "is-active" : null)">
                            <div>
                                <div class="category-management__category-name">@category.Name</div>
                                <div class="category-management__category-meta">@category.ProductCount san pham</div>
                            </div>
                            <span class="category-management__category-code">@category.Id.ToString("D2")</span>
                        </a>
                    }
                </div>
            </div>
        </aside>

        <div class="category-management__workspace">
            <div class="category-management__panel">
                <div class="category-management__workspace-head">
                    <div>
                        <span class="category-pill mb-3">@selectedCategory!.Id.ToString("D2")</span>
                        <h2 class="section-title mt-3 mb-2">@selectedCategory.Name</h2>
                        <p class="section-copy mb-0">Danh sach san pham trong danh muc nay duoc quan ly truc tiep tu workspace hien tai.</p>
                    </div>
                    <div class="management-actions">
                        <a asp-action="Display" asp-route-id="@selectedCategory.Id" class="btn btn-sm btn-primary">Xem</a>
                        <a asp-action="Update" asp-route-id="@selectedCategory.Id" class="btn btn-sm btn-outline-primary">Sua</a>
                        <a asp-action="Delete" asp-route-id="@selectedCategory.Id" class="btn btn-sm btn-outline-danger">Xoa</a>
                    </div>
                </div>

                <div class="category-management__summary">
                    <article class="category-management__summary-card">
                        <span class="management-meta">So luong san pham</span>
                        <strong>@Model.SelectedProductCount</strong>
                    </article>
                    <article class="category-management__summary-card">
                        <span class="management-meta">Dieu huong nhanh</span>
                        <a asp-controller="Product" asp-action="Index" asp-route-categoryId="@selectedCategory.Id">Mo catalog loc theo danh muc</a>
                    </article>
                </div>

                @if (Model.Products.Count == 0)
                {
                    <section class="empty-state category-management__empty">
                        <h3 class="section-title">Danh muc nay chua co san pham.</h3>
                        <p class="section-copy mx-auto mt-3 mb-4">Tao san pham dau tien trong danh muc hien tai de bat dau quan ly theo nhom.</p>
                        <a asp-controller="Product" asp-action="Add" asp-route-categoryId="@selectedCategory.Id" asp-route-returnUrl="@returnUrl" class="btn btn-electric">Them san pham dau tien</a>
                    </section>
                }
                else
                {
                    <div class="category-management__product-table-wrap">
                        <table class="table category-management__product-table">
                            <thead>
                                <tr>
                                    <th>San pham</th>
                                    <th>Gia</th>
                                    <th>Anh</th>
                                    <th class="text-end">Thao tac</th>
                                </tr>
                            </thead>
                            <tbody>
                                @foreach (var product in Model.Products)
                                {
                                    <tr>
                                        <td>
                                            <div class="category-management__product-name">@product.Name</div>
                                            <div class="management-meta">@product.Description</div>
                                        </td>
                                        <td>@product.Price.ToString("N0")đ</td>
                                        <td>@(string.IsNullOrWhiteSpace(product.ImageUrl) ? "Chua co" : "Da co")</td>
                                        <td class="text-end">
                                            <div class="management-actions justify-content-end">
                                                <a asp-controller="Product" asp-action="Display" asp-route-id="@product.Id" class="btn btn-sm btn-primary">Xem</a>
                                                <a asp-controller="Product" asp-action="Update" asp-route-id="@product.Id" asp-route-returnUrl="@returnUrl" class="btn btn-sm btn-outline-primary">Sua</a>
                                                <a asp-controller="Product" asp-action="Delete" asp-route-id="@product.Id" asp-route-returnUrl="@returnUrl" class="btn btn-sm btn-outline-danger">Xoa</a>
                                            </div>
                                        </td>
                                    </tr>
                                }
                            </tbody>
                        </table>
                    </div>
                }
            </div>
        </div>
    </section>
}
```

- [ ] **Step 2: Add scoped CSS for the new management layout**

Append to `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`:

```css
.category-management {
  display: grid;
  grid-template-columns: minmax(260px, 320px) minmax(0, 1fr);
  gap: 24px;
}

.category-management__panel {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 24px;
}

.category-management__panel-head,
.category-management__workspace-head,
.category-management__summary {
  display: flex;
  gap: 16px;
}

.category-management__panel-head,
.category-management__workspace-head {
  align-items: flex-start;
  justify-content: space-between;
}

.category-management__summary {
  margin-top: 20px;
  margin-bottom: 20px;
  flex-wrap: wrap;
}

.category-management__summary-card {
  flex: 1 1 220px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 16px;
  background: #f8fafc;
}

.category-management__summary-card strong {
  display: block;
  margin-top: 8px;
  font-size: 28px;
  line-height: 1.2;
}

.category-management__category-list {
  display: grid;
  gap: 12px;
  margin-top: 20px;
}

.category-management__category-item {
  display: flex;
  justify-content: space-between;
  gap: 16px;
  padding: 16px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  text-decoration: none;
  color: inherit;
  background: #fff;
}

.category-management__category-item.is-active {
  border-color: #ff6b00;
  background: #fff7f2;
}

.category-management__category-name,
.category-management__product-name {
  font-weight: 600;
  color: #0f172a;
}

.category-management__category-code {
  font-family: "JetBrains Mono", monospace;
  font-size: 12px;
  color: #64748b;
}

.category-management__product-table-wrap {
  overflow-x: auto;
}

.category-management__product-table {
  margin-bottom: 0;
}

.category-management__product-table thead th {
  background: #0f172a;
  color: #fff;
  border: 0;
}

.category-management__product-table td,
.category-management__product-table th {
  vertical-align: middle;
}

.category-management__product-table tbody tr:nth-child(even) {
  background: #f8fafc;
}

.category-management__empty {
  margin-top: 16px;
}

@media (max-width: 991.98px) {
  .category-management {
    grid-template-columns: 1fr;
  }

  .category-management__panel-head,
  .category-management__workspace-head {
    flex-direction: column;
  }
}
```

- [ ] **Step 3: Build to verify the view and CSS compile cleanly**

Run:

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build succeeds.

- [ ] **Step 4: Commit**

If `.git` is restored later:

```powershell
git add WebCongNghe\Views\Categories\Index.cshtml WebCongNghe\wwwroot\css\site.css
git commit -m "feat: add category product management workspace"
```

### Task 5: Verify Category-Oriented CRUD Flow End To End

**Files:**
- Modify: none

- [ ] **Step 1: Start the app locally**

Run:

```powershell
dotnet run --project M:\WebCongNghe\WebCongNghe\WebCongNghe.csproj --urls http://localhost:5057
```

Expected: application starts on `http://localhost:5057`.

- [ ] **Step 2: Verify category workspace renders**

Open:

```text
http://localhost:5057/Categories
```

Expected:

- category rail appears
- one category is selected automatically
- selected category workspace shows products

- [ ] **Step 3: Verify switching category**

Open a second category from the left rail.

Expected:

- URL includes `selectedCategoryId=...`
- right panel updates to that category
- product count and product list change accordingly

- [ ] **Step 4: Verify add-product context**

From the selected category workspace, click `Them san pham`.

Expected:

- add form opens with category preselected
- submitting the form returns to the selected category workspace
- the new product appears in that category list

- [ ] **Step 5: Verify update/delete context**

From the selected category workspace:

- click `Sua` for a product
- submit an edit
- click `Xoa` for a product and confirm

Expected:

- both flows return to the same category workspace
- edited data is visible
- deleted product disappears from the list

- [ ] **Step 6: Optional HTTP smoke check**

Run:

```powershell
Invoke-WebRequest http://localhost:5057/Categories -UseBasicParsing | Select-Object -ExpandProperty StatusCode
```

Expected: `200`

- [ ] **Step 7: Commit**

If `.git` is restored later:

```powershell
git add .
git commit -m "test: verify category product management flow"
```

