# Category Product List Visual Refinement

## Goal

Refine the product list inside the category management workspace so each product row shows the real product image and the CRUD actions are arranged more compactly.

The result should remain an admin-oriented management surface, not a storefront card grid.

## Scope

In scope:

- show product thumbnail in the category workspace product list
- reorganize CRUD controls to be more compact and visually aligned
- keep current category-product management flow unchanged

Out of scope:

- changing controller behavior
- changing database schema
- redesigning the whole category workspace
- changing storefront catalog cards

## Current Problem

The current table:

- only shows `Da co / Chua co` instead of previewing the real image
- uses a tall action stack that wastes horizontal and vertical space
- makes scanning products harder than necessary

## Recommended Approach

Keep the table-based admin layout, but upgrade each product row:

- add a fixed-size thumbnail preview
- group product name and description in a clearer media block
- compress `Xem / Sua / Xoa` into one horizontal action cluster

This keeps the interface dense and management-focused while improving readability.

## UI Changes

### Product row

Each product row should include:

- thumbnail image
- product name
- short description
- price
- image presence status if still useful
- compact CRUD actions

### Image behavior

- use `product.ImageUrl` when present
- if missing, show a clean placeholder thumbnail
- thumbnail size should be fixed so rows stay aligned

### CRUD actions

- `Xem` remains the primary action
- `Sua` and `Xoa` become smaller secondary actions in the same row
- actions should align horizontally on desktop
- on narrow screens they may wrap, but should stay grouped in one compact block

## Files Expected To Change

- `WebCongNghe/Views/Categories/Index.cshtml`
- `WebCongNghe/wwwroot/css/site.css`

## Verification

Implementation is complete when:

- product rows display thumbnails
- action controls look compact and aligned
- build passes
- `/Categories` renders successfully

