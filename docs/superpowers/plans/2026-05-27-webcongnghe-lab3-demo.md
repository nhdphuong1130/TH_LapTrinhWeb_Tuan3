# WebCongNghe Lab 3 Demo Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a demo ASP.NET Core MVC shopping site for Lab 3 with SQL Server LocalDB, Entity Framework Core, repositories, CRUD for products and categories, and single-image upload.

**Architecture:** The app keeps the default MVC structure and adds EF Core for persistence. Products and categories are modeled with a small repository layer so the implementation stays close to the course material while remaining runnable in the existing project.

**Tech Stack:** ASP.NET Core MVC (.NET 10), Entity Framework Core SQL Server, LocalDB, Razor Views, Bootstrap

---

### Task 1: Prepare project structure

**Files:**
- Create: `docs/superpowers/plans/2026-05-27-webcongnghe-lab3-demo.md`
- Create: `WebCongNghe.Tests/WebCongNghe.Tests.csproj`
- Create: `WebCongNghe.Tests/SmokeTests.cs`

- [ ] Add a test project and a minimal failing smoke test for the solution.
- [ ] Run the targeted test and verify the failure.
- [ ] Adjust project references and make the smoke test pass.

### Task 2: Add data access layer

**Files:**
- Modify: `WebCongNghe/WebCongNghe.csproj`
- Modify: `WebCongNghe/appsettings.json`
- Modify: `WebCongNghe/Program.cs`
- Create: `WebCongNghe/Data/ApplicationDbContext.cs`
- Create: `WebCongNghe/Models/Category.cs`
- Create: `WebCongNghe/Models/Product.cs`
- Create: `WebCongNghe/Models/ProductImage.cs`
- Create: `WebCongNghe/Repositories/IProductRepository.cs`
- Create: `WebCongNghe/Repositories/ICategoryRepository.cs`
- Create: `WebCongNghe/Repositories/EFProductRepository.cs`
- Create: `WebCongNghe/Repositories/EFCategoryRepository.cs`

- [ ] Add EF Core packages and connection string for LocalDB.
- [ ] Create domain models and `ApplicationDbContext`.
- [ ] Register DbContext and repositories in DI.

### Task 3: Build product and category CRUD

**Files:**
- Create: `WebCongNghe/Controllers/ProductController.cs`
- Create: `WebCongNghe/Controllers/CategoriesController.cs`
- Create: `WebCongNghe/Views/Product/*.cshtml`
- Create: `WebCongNghe/Views/Categories/*.cshtml`
- Modify: `WebCongNghe/Views/Shared/_Layout.cshtml`
- Modify: `WebCongNghe/Views/Home/Index.cshtml`
- Modify: `WebCongNghe/wwwroot/css/site.css`

- [ ] Add controller actions for list, add, edit, details, and delete.
- [ ] Create Razor views with validation and category selection.
- [ ] Add product image upload and client-side preview on edit.

### Task 4: Verify persistence and runtime

**Files:**
- Create: `WebCongNghe/Migrations/*`

- [ ] Create the initial migration.
- [ ] Update the LocalDB database.
- [ ] Run build and targeted verification commands.
