# Category Product List Visual Refinement Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Refine the product rows inside the category management workspace to show real thumbnails and compact CRUD controls.

**Architecture:** Keep the existing category management controller flow unchanged and limit the work to the Razor view plus scoped CSS. Replace the current text-only image status cell with a thumbnail/media block and turn the stacked actions into one compact horizontal cluster.

**Tech Stack:** ASP.NET Core MVC (.NET 8), Razor Views, scoped CSS in `wwwroot/css/site.css`

---

## File Structure

- Modify: `M:\WebCongNghe\WebCongNghe\Views\Categories\Index.cshtml`
  - Add thumbnail markup and compact action block per product row.
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`
  - Add row media layout, thumbnail sizing, placeholder styling, and compact action styling.

### Task 1: Refine Product Row Markup

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\Views\Categories\Index.cshtml`

- [ ] **Step 1: Replace the product table row structure**

Inside the existing `foreach (var product in Model.Products)` block, replace the current cells with:

```cshtml
<tr>
    <td>
        <div class="category-management__product-cell">
            <div class="category-management__thumb-wrap">
                @if (!string.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    <img src="@product.ImageUrl" alt="@product.Name" class="category-management__thumb" />
                }
                else
                {
                    <div class="category-management__thumb-placeholder">No image</div>
                }
            </div>
            <div class="category-management__product-copy">
                <div class="category-management__product-name">@product.Name</div>
                <div class="management-meta">@(string.IsNullOrWhiteSpace(product.Description) ? "Khong co mo ta bo sung." : product.Description)</div>
            </div>
        </div>
    </td>
    <td>
        <div class="category-management__price">@product.Price.ToString("N0") VND</div>
    </td>
    <td>
        <span class="category-management__image-status @(string.IsNullOrWhiteSpace(product.ImageUrl) ? "is-empty" : "is-ready")">
            @(string.IsNullOrWhiteSpace(product.ImageUrl) ? "Chua co" : "Da co")
        </span>
    </td>
    <td class="text-end">
        <div class="category-management__product-actions">
            <a asp-controller="Product" asp-action="Display" asp-route-id="@product.Id" class="btn btn-sm btn-primary">Xem</a>
            <a asp-controller="Product" asp-action="Update" asp-route-id="@product.Id" asp-route-returnUrl="@returnUrl" class="btn btn-sm btn-outline-primary">Sua</a>
            <a asp-controller="Product" asp-action="Delete" asp-route-id="@product.Id" asp-route-returnUrl="@returnUrl" class="btn btn-sm btn-outline-danger">Xoa</a>
        </div>
    </td>
</tr>
```

- [ ] **Step 2: Keep table headers compatible with the new row layout**

Keep the current table headers, but ensure they still read:

```cshtml
<tr>
    <th>San pham</th>
    <th>Gia</th>
    <th>Anh</th>
    <th class="text-end">Thao tac</th>
</tr>
```

- [ ] **Step 3: Build to catch Razor errors early**

Run:

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build succeeds, or only CSS/layout work remains if markup references classes not yet styled.

### Task 2: Add Compact Thumbnail and Action Styles

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`

- [ ] **Step 1: Add product media layout styles**

Append these rules near the existing `.category-management__product-*` styles:

```css
.category-management__product-cell {
  display: grid;
  grid-template-columns: 72px minmax(0, 1fr);
  gap: 14px;
  align-items: center;
}

.category-management__thumb-wrap {
  width: 72px;
  height: 72px;
  flex: 0 0 72px;
}

.category-management__thumb,
.category-management__thumb-placeholder {
  width: 72px;
  height: 72px;
  border-radius: 8px;
}

.category-management__thumb {
  object-fit: cover;
  border: 1px solid #e2e8f0;
  background: #fff;
}

.category-management__thumb-placeholder {
  display: grid;
  place-items: center;
  background: #f8fafc;
  border: 1px dashed #cbd5e1;
  color: #64748b;
  font-size: 12px;
  text-transform: uppercase;
}

.category-management__product-copy {
  min-width: 0;
}

.category-management__price {
  font-weight: 600;
  color: #0f172a;
}
```

- [ ] **Step 2: Add image status and compact action styling**

Append:

```css
.category-management__image-status {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 72px;
  padding: 6px 10px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 600;
}

.category-management__image-status.is-ready {
  background: rgba(22, 163, 74, 0.12);
  color: #15803d;
}

.category-management__image-status.is-empty {
  background: rgba(148, 163, 184, 0.14);
  color: #475569;
}

.category-management__product-actions {
  display: inline-flex;
  align-items: center;
  justify-content: flex-end;
  gap: 8px;
  flex-wrap: wrap;
}

.category-management__product-actions .btn {
  min-width: 0;
}
```

- [ ] **Step 3: Add responsive behavior for narrow screens**

Append inside the existing mobile admin breakpoint or a new matching media query:

```css
@media (max-width: 767.98px) {
  .category-management__product-cell {
    grid-template-columns: 56px minmax(0, 1fr);
    gap: 12px;
  }

  .category-management__thumb-wrap,
  .category-management__thumb,
  .category-management__thumb-placeholder {
    width: 56px;
    height: 56px;
  }

  .category-management__product-actions {
    justify-content: flex-start;
  }
}
```

- [ ] **Step 4: Build to verify the CSS changes did not introduce view issues**

Run:

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build succeeds.

### Task 3: Verify Rendering

**Files:**
- Modify: none

- [ ] **Step 1: Start or reuse the local app**

Run if needed:

```powershell
dotnet run --project M:\WebCongNghe\WebCongNghe\WebCongNghe.csproj --urls http://localhost:5057
```

Expected: app responds on `http://localhost:5057`.

- [ ] **Step 2: Verify category workspace still renders**

Run:

```powershell
Invoke-WebRequest http://localhost:5057/Categories -UseBasicParsing | Select-Object -ExpandProperty StatusCode
```

Expected: `200`

- [ ] **Step 3: Verify thumbnails and action cluster exist in HTML**

Run:

```powershell
$response = Invoke-WebRequest 'http://localhost:5057/Categories?selectedCategoryId=3' -UseBasicParsing
$content = $response.Content
[PSCustomObject]@{
  HasThumbClass = $content -match 'category-management__thumb'
  HasActionsClass = $content -match 'category-management__product-actions'
} | ConvertTo-Json -Compress
```

Expected:

```json
{"HasThumbClass":true,"HasActionsClass":true}
```

