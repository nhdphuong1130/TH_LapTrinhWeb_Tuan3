# WebCongNghe Lab 3 UI Redesign Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Redesign the Lab 3 MVC demo into a darker, more polished admin UI while keeping the existing backend and CRUD flows intact.

**Architecture:** Keep all controller and data-access behavior unchanged and rebuild the presentation layer through Razor views and shared CSS. Use one shared visual system in `site.css`, a redesigned shell in `_Layout.cshtml`, and page-specific markup updates across `Home`, `Product`, and `Categories`.

**Tech Stack:** ASP.NET Core MVC, Razor Views, Bootstrap, custom CSS, xUnit

---

### Task 1: Lock the new UI contract with tests

**Files:**
- Modify: `WebCongNghe.Tests/SmokeTests.cs`
- Test: `WebCongNghe.Tests/SmokeTests.cs`

- [ ] Add a failing test that checks the layout contains the new brand and navigation labels.
- [ ] Run the targeted test and confirm it fails for the expected missing UI markers.
- [ ] Add a failing test that checks the home page contains the new dashboard messaging.
- [ ] Run the targeted test and confirm it fails for the expected missing content.

### Task 2: Redesign the shared shell and visual system

**Files:**
- Modify: `WebCongNghe/Views/Shared/_Layout.cshtml`
- Modify: `WebCongNghe/wwwroot/css/site.css`

- [ ] Replace the basic Bootstrap shell with a dark admin shell.
- [ ] Add global styles for navigation, page headers, surfaces, tables, forms, buttons, and responsive behavior.
- [ ] Run the UI smoke tests and confirm at least the layout-oriented checks pass.

### Task 3: Rebuild the home dashboard

**Files:**
- Modify: `WebCongNghe/Views/Home/Index.cshtml`

- [ ] Replace the simple hero copy with a dashboard intro and action blocks.
- [ ] Add implementation-status content matching the approved redesign spec.
- [ ] Run the targeted home-page test and confirm it passes.

### Task 4: Rebuild product management views

**Files:**
- Modify: `WebCongNghe/Views/Product/Index.cshtml`
- Modify: `WebCongNghe/Views/Product/Add.cshtml`
- Modify: `WebCongNghe/Views/Product/Update.cshtml`
- Modify: `WebCongNghe/Views/Product/Display.cshtml`
- Modify: `WebCongNghe/Views/Product/Delete.cshtml`

- [ ] Redesign the product list as a polished management table.
- [ ] Redesign add/update into a denser two-column workflow with preview support.
- [ ] Redesign display/delete into consistent detail and confirmation surfaces.

### Task 5: Rebuild category management views

**Files:**
- Modify: `WebCongNghe/Views/Categories/Index.cshtml`
- Modify: `WebCongNghe/Views/Categories/Add.cshtml`
- Modify: `WebCongNghe/Views/Categories/Update.cshtml`
- Modify: `WebCongNghe/Views/Categories/Display.cshtml`
- Modify: `WebCongNghe/Views/Categories/Delete.cshtml`

- [ ] Apply the same design system to category list and form pages.
- [ ] Keep category pages visually lighter but structurally consistent with product pages.

### Task 6: Verify runtime behavior

**Files:**
- Verify only

- [ ] Run `dotnet build` for the web project.
- [ ] Run `dotnet test` for the test project.
- [ ] Start the app locally and verify `/`, `/Product`, and `/Categories` return HTTP 200.
