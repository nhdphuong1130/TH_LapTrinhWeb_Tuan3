# Category Workspace Width And Actions Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make the category workspace use width more effectively and keep product CRUD actions on a single row on desktop/tablet layouts.

**Architecture:** Limit the change to CSS. Rebalance the grid so the workspace gets more room, constrain product table columns more deliberately, and disable action wrapping above mobile breakpoints.

**Tech Stack:** ASP.NET Core MVC (.NET 8), Razor-rendered HTML, scoped CSS in `wwwroot/css/site.css`

---

## File Structure

- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`
  - Adjust category workspace grid, table column behavior, and action wrapping rules.

### Task 1: Rebalance Workspace Width

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`

- [ ] **Step 1: Increase width priority for the right workspace**

Update the existing `.category-management` rule from:

```css
.category-management {
  display: grid;
  grid-template-columns: minmax(260px, 320px) minmax(0, 1fr);
  gap: 24px;
}
```

to:

```css
.category-management {
  display: grid;
  grid-template-columns: minmax(220px, 280px) minmax(0, 1.35fr);
  gap: 24px;
}
```

- [ ] **Step 2: Make the product table use fixed layout**

Add or update:

```css
.category-management__product-table {
  width: 100%;
  margin-bottom: 0;
  table-layout: fixed;
}
```

- [ ] **Step 3: Assign tighter widths to non-action columns**

Append:

```css
.category-management__product-table th:nth-child(2),
.category-management__product-table td:nth-child(2) {
  width: 140px;
}

.category-management__product-table th:nth-child(3),
.category-management__product-table td:nth-child(3) {
  width: 110px;
}

.category-management__product-table th:nth-child(4),
.category-management__product-table td:nth-child(4) {
  width: 190px;
  white-space: nowrap;
}
```

### Task 2: Keep CRUD Actions On One Row

**Files:**
- Modify: `M:\WebCongNghe\WebCongNghe\wwwroot\css\site.css`

- [ ] **Step 1: Prevent wrapping for action cluster on larger screens**

Update the current `.category-management__product-actions` rule to:

```css
.category-management__product-actions {
  display: inline-flex;
  align-items: center;
  justify-content: flex-end;
  gap: 8px;
  flex-wrap: nowrap;
  white-space: nowrap;
}
```

- [ ] **Step 2: Keep mobile wrapping behavior only at small widths**

Inside the existing `@media (max-width: 767.98px)` block, add:

```css
.category-management__product-table th:nth-child(2),
.category-management__product-table td:nth-child(2),
.category-management__product-table th:nth-child(3),
.category-management__product-table td:nth-child(3),
.category-management__product-table th:nth-child(4),
.category-management__product-table td:nth-child(4) {
  width: auto;
  white-space: normal;
}

.category-management__product-actions {
  flex-wrap: wrap;
  white-space: normal;
}
```

### Task 3: Verify Rendering

**Files:**
- Modify: none

- [ ] **Step 1: Stop any running app instance if needed**

Run:

```powershell
Get-Process WebCongNghe -ErrorAction SilentlyContinue | Stop-Process -Force
```

- [ ] **Step 2: Build the solution**

Run:

```powershell
dotnet build M:\WebCongNghe\WebCongNghe.sln
```

Expected: build succeeds.

- [ ] **Step 3: Start the app**

Run:

```powershell
dotnet run --project M:\WebCongNghe\WebCongNghe\WebCongNghe.csproj --urls http://localhost:5057
```

Expected: app responds on `http://localhost:5057`.

- [ ] **Step 4: Verify category workspace response**

Run:

```powershell
Invoke-WebRequest http://localhost:5057/Categories -UseBasicParsing | Select-Object -ExpandProperty StatusCode
```

Expected: `200`

- [ ] **Step 5: Verify desktop-oriented action layout classes exist**

Run:

```powershell
$response = Invoke-WebRequest 'http://localhost:5057/Categories?selectedCategoryId=3' -UseBasicParsing
$content = $response.Content
[PSCustomObject]@{
  HasProductActions = $content -match 'category-management__product-actions'
  HasProductTable = $content -match 'category-management__product-table'
} | ConvertTo-Json -Compress
```

Expected:

```json
{"HasProductActions":true,"HasProductTable":true}
```

