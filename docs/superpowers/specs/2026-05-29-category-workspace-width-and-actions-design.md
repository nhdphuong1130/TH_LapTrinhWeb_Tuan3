# Category Workspace Width And Actions Design

## Goal

Refine the category management workspace so the product table uses the available width more effectively and keeps the product CRUD controls on a single row in desktop-sized layouts.

## Scope

In scope:

- widen the effective product workspace area
- reduce table column pressure
- keep product CRUD controls on one row for desktop and tablet widths

Out of scope:

- changing controller behavior
- redesigning the entire category management page
- changing mobile-first wrapping behavior where horizontal space is genuinely limited

## Current Problem

The current workspace still has enough visible width, but the table layout is not allocating that width well. As a result, the product action controls wrap or stack even though the screen can still accommodate a single-row action cluster.

## Recommended Approach

Keep the current category management structure and adjust only layout constraints:

- give more width priority to the right workspace
- constrain the non-action table columns more deliberately
- force the CRUD action cluster to remain on one row on larger screens

## Expected Result

- product rows feel less cramped
- the right workspace uses width more efficiently
- `Xem / Sua / Xoa` remains on one row on desktop and tablet
- wrapping is allowed only on narrow mobile layouts

## Files Expected To Change

- `WebCongNghe/wwwroot/css/site.css`

## Verification

Implementation is complete when:

- the category workspace renders successfully
- CRUD buttons stay on one row on desktop-width rendering
- build passes

