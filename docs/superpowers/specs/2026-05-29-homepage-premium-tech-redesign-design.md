# Homepage Premium Tech Redesign

## Goal

Redesign the homepage to follow the reference layout pattern from the user's sample while aligning visual decisions with `C:\Users\Administrator\Downloads\DESIGN.md`.

The homepage should feel like a premium technology storefront, not a generic class demo or admin dashboard. It must use real project data from the existing SQL-backed catalog.

## Scope

In scope:

- Replace the current homepage layout entirely
- Keep the work limited to homepage-specific controller, view model, Razor view, and scoped CSS
- Use real product and category data from the application database
- Preserve existing routes and project structure

Out of scope:

- Redesigning shared header/footer shell
- Changing CRUD pages
- Adding new database schema
- Adding new business workflows beyond homepage display

## Layout

The new homepage will use these sections in order:

1. Hero
2. Trust signals strip
3. Featured categories
4. Flash sale / featured products
5. Brand story section
6. Newsletter / CTA block

The structure should follow the visual rhythm of the user's sample, but with a stricter `premium tech corporate` treatment from `DESIGN.md`.

## Data Model

A dedicated `HomeViewModel` will be introduced so the controller prepares all homepage data before rendering.

The view model should include:

- `FeaturedHeroProduct`
- `FeaturedCategories`
- `FlashSaleProducts`
- optional small supporting values for counts or labels if needed by the view

The Razor view should remain presentation-focused and avoid doing catalog filtering logic inline.

## Data Selection Rules

### Hero product

Pick one real product from the database with the strongest homepage visual potential.

Selection preference:

1. product with image and `OriginalPrice > Price`
2. otherwise highest-priced product with image

### Featured categories

Render up to 4 category cards.

Each category card should use:

- real category name
- link to filtered product catalog
- cover image from one product inside that category

If fewer than 4 categories exist, reuse existing categories without inventing fake ones.

### Flash sale products

Render 4 real products.

Selection preference:

1. products with discount (`OriginalPrice > Price`)
2. fallback to products with strong images and meaningful pricing

The section should show:

- image
- name
- current price
- original price when present
- add-to-cart action
- link to detail page

## Visual Direction

The homepage must follow `DESIGN.md`:

- light background
- white cards
- dark slate typography
- orange `#FF6B00` for CTA and price emphasis
- purple accent used sparingly for technical cues only
- disciplined 4px / 8px radii
- soft ambient shadows instead of heavy gradients

Specific adjustments from the sample:

- reduce decorative marketing feel
- sharpen card and button geometry
- increase whitespace consistency
- keep imagery prominent without oversized ornamental effects

## Interaction

- Primary CTA should lead to product catalog
- Secondary CTA can lead to category management or featured product detail depending on section
- Category cards should be clickable
- Flash sale cards should support add-to-cart and detail navigation

## Error Handling

If data is sparse:

- hero should still render with best available product
- categories section should render only available categories
- flash section should render fewer cards rather than invent placeholders

The homepage must not fail because a category or product image is missing; it should fall back to the existing placeholder image path already used by the catalog when needed.

## Files Expected To Change

- `WebCongNghe/Controllers/HomeController.cs`
- `WebCongNghe/Models/HomeViewModel.cs` or equivalent homepage view model file
- `WebCongNghe/Views/Home/Index.cshtml`
- `WebCongNghe/wwwroot/css/site.css`

## Verification

Implementation will be considered complete when:

- homepage renders successfully
- build passes on `.NET 8`
- homepage uses real DB data
- catalog/detail/cart links from homepage work
- homepage styling is visually consistent with `DESIGN.md`

