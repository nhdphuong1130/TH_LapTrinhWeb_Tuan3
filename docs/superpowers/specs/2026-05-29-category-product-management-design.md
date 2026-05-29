# Category Product Management Design

## Goal

Build a category management experience where the user enters the category area, sees the existing categories, selects one category, and manages the products that belong to that category from the same workflow.

The feature should feel like a focused backoffice screen, not a separate disconnected set of CRUD pages.

## Scope

In scope:

- Turn category management into a master-detail screen
- Show the list of existing categories
- Allow selecting a category
- Show products that belong to the selected category
- Allow product CRUD from within the selected category flow
- Preserve the current database relationship: `Product -> CategoryId`

Out of scope:

- Adding category hierarchy
- Adding category types or subcategories
- Redesigning the storefront catalog
- Changing product schema

## Current Context

The codebase currently has:

- `Category` as a flat entity
- `Product` linked by `CategoryId`
- `CategoriesController` handling category CRUD
- `ProductController` handling product CRUD
- a standalone `Categories/Index` list and separate product catalog/admin flows

This already supports the required data relationship, so the change should be made at the controller/view flow level rather than at the database schema level.

## Recommended Approach

Use a master-detail management screen in `Categories/Index`.

- Left side: category list
- Right side: selected category details and the products belonging to it
- Product actions remain routed through the existing product controller where practical

This gives the user one clear entry point for category-oriented management without introducing duplicate business logic.

## Interaction Model

### Entry

When opening `Danh muc`, the page should load all categories and auto-select one category:

1. use `selectedCategoryId` from query string if present
2. otherwise select the first available category

### Category selection

Clicking a category refreshes the same screen and loads:

- category summary
- product count
- products in that category

### Product management from selected category

From the selected category panel, the user can:

- add a new product for that category
- open product detail
- open update
- open delete

The add-product entry should prefill or lock the selected `CategoryId` so the product is created in the current category context.

### Empty state

If a category has no products:

- show a clear empty state
- show a primary CTA to add the first product for that category

If there are no categories at all:

- keep the current category empty-state behavior
- primary CTA remains create category

## Data Shape

Add a dedicated view model for the category management screen so the Razor view stays presentation-focused.

Recommended structure:

- collection of categories with selection state
- selected category summary
- selected category product list
- total count information for category and product panels

This avoids passing multiple unrelated objects through `ViewBag` and keeps controller logic explicit.

## Routing and Controller Strategy

### CategoriesController

`Index` should become the main orchestration endpoint for category-oriented management.

It should:

- load all categories
- resolve selected category
- load products for the selected category
- return a typed view model

### ProductController

Keep product CRUD in `ProductController`, but support category-oriented entry:

- `Add` should accept an optional `categoryId`
- post-add redirect should be able to return to the selected category management screen
- update/delete flows should preserve a return path back to the selected category screen when entered from there

The goal is reuse of existing product CRUD instead of duplicating product business logic inside `CategoriesController`.

## UI Structure

The page should use a two-column admin layout:

1. category rail / list
2. selected category workspace

The workspace should contain:

- category title
- small metadata such as product count
- action buttons for category edit/delete if needed
- a compact product table or management card list
- add-product CTA at the top

The page should remain consistent with the existing backoffice visual language already used in the project.

## Error Handling

- invalid `selectedCategoryId` should fall back to the first available category
- missing category on edit/delete should still return `NotFound()`
- product actions entered from category management should preserve context safely through route values or return URL

## Testing and Verification

Implementation is complete when:

- opening `Danh muc` shows categories and a selected category workspace
- selecting another category changes the product list
- adding a product from that workspace assigns it to the selected category
- updating and deleting products still work
- redirects return the user to the category management view when applicable
- build passes and the page renders successfully

