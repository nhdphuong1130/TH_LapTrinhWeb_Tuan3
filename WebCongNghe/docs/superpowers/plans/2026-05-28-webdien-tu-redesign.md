# WebDienTu Redesign Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Rebuild the ASP.NET Core MVC UI into a bright-tech storefront and aligned CRUD/admin experience without changing existing backend behavior.

**Architecture:** Keep the current MVC structure and shared layout, but replace the visual system in `site.css`, refresh the shared shell in `_Layout.cshtml`, and refactor the Razor views so storefront and CRUD pages share the same design language. Keep data flow and controller actions intact, only changing markup structure and styling hooks.

**Tech Stack:** ASP.NET Core MVC, Razor views, Bootstrap, custom CSS, existing model/controller layer

---

## File Structure

- Modify: `Views/Shared/_Layout.cshtml`
  - Rebuild the shared header, navigation, main shell, and richer footer.
- Modify: `wwwroot/css/site.css`
  - Replace the current mixed dark/purple layer with the approved bright-tech token and component system.
- Modify: `Views/Home/Index.cshtml`
  - Convert the current dashboard-like landing page into a marketing storefront home page.
- Modify: `Views/Product/Index.cshtml`
  - Replace table-first presentation with a storefront catalog grid while preserving admin actions.
- Modify: `Views/Product/Display.cshtml`
  - Rework the product detail view into a merchandising layout.
- Modify: `Views/Product/Add.cshtml`
  - Refresh the create form as a cleaner admin/editor surface.
- Modify: `Views/Product/Update.cshtml`
  - Refresh the edit form and preserve image preview behavior.
- Modify: `Views/Product/Delete.cshtml`
  - Rework delete confirmation into a stronger decision layout.
- Modify: `Views/Categories/Index.cshtml`
  - Refresh the category index into a cleaner admin listing.
- Modify: `Views/Categories/Display.cshtml`
  - Rework category detail into a summary card layout.
- Modify: `Views/Categories/Add.cshtml`
  - Align create form with the new editor language.
- Modify: `Views/Categories/Update.cshtml`
  - Align update form with the new editor language.
- Modify: `Views/Categories/Delete.cshtml`
  - Rework delete confirmation into the shared destructive-action pattern.

## Task 1: Rebuild Shared Shell And Token Layer

**Files:**
- Modify: `Views/Shared/_Layout.cshtml`
- Modify: `wwwroot/css/site.css`

- [ ] Step 1: Replace the current shared shell markup with a stronger storefront header and richer footer while keeping existing routes.
- [ ] Step 2: Rebuild CSS variables, background layers, typography hierarchy, buttons, cards, shell sections, and responsive rules in `wwwroot/css/site.css`.
- [ ] Step 3: Add shared utility/component classes that both store and CRUD views can reuse, including hero blocks, grids, chips, form panels, and management cards.

## Task 2: Redesign Storefront Landing Page

**Files:**
- Modify: `Views/Home/Index.cshtml`
- Modify: `wwwroot/css/site.css`

- [ ] Step 1: Replace the current dashboard copy/sections with a sales-focused hero, trust chips, highlight cards, and CTA group.
- [ ] Step 2: Add a category discovery section and featured selling points that can render well even without dynamic category querying.
- [ ] Step 3: Keep the page data-light and marketing-forward, using static presentation copy where needed without affecting backend behavior.

## Task 3: Redesign Product Catalog And Detail

**Files:**
- Modify: `Views/Product/Index.cshtml`
- Modify: `Views/Product/Display.cshtml`
- Modify: `wwwroot/css/site.css`

- [ ] Step 1: Convert the product index from a pure management table into a storefront catalog grid with cards, price emphasis, category labels, and embedded CRUD actions.
- [ ] Step 2: Preserve empty-state handling when no products exist.
- [ ] Step 3: Rebuild the product detail page into an image-first merchandising layout with structured metadata and clear actions.

## Task 4: Redesign Category Listing And Detail

**Files:**
- Modify: `Views/Categories/Index.cshtml`
- Modify: `Views/Categories/Display.cshtml`
- Modify: `wwwroot/css/site.css`

- [ ] Step 1: Refresh category index into a lighter management view consistent with the new admin language.
- [ ] Step 2: Rework category detail into summary cards and supporting context instead of a plain block.

## Task 5: Redesign Product And Category Editor Flows

**Files:**
- Modify: `Views/Product/Add.cshtml`
- Modify: `Views/Product/Update.cshtml`
- Modify: `Views/Product/Delete.cshtml`
- Modify: `Views/Categories/Add.cshtml`
- Modify: `Views/Categories/Update.cshtml`
- Modify: `Views/Categories/Delete.cshtml`
- Modify: `wwwroot/css/site.css`

- [ ] Step 1: Align create/update forms to a shared editor layout with sectioned panels, better helper copy, and stronger field rhythm.
- [ ] Step 2: Preserve current product image upload and preview behavior in the update form.
- [ ] Step 3: Rebuild delete screens into shared destructive confirmation layouts.

## Task 6: Verification

**Files:**
- Modify as needed: affected Razor/CSS files above

- [ ] Step 1: Run `dotnet build` from `WebCongNghe/WebCongNghe` to catch Razor/C# issues after markup changes.
- [ ] Step 2: Fix any compile or Razor binding errors introduced by the redesign.
- [ ] Step 3: Review responsive/layout-sensitive sections in code and tighten CSS where obvious overflow or wrapping issues remain.

## Self-Review

- Spec coverage: covered shared shell, storefront home, product listing/detail, category listing/detail, product/category add/update/delete, responsive consistency, and verification.
- Placeholder scan: no TBD/TODO markers remain.
- Type consistency: all tasks reference existing Razor views and shared CSS only; no new backend APIs or renamed model properties are introduced.
