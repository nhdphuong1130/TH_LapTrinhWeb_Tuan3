# WebDienTu Redesign Spec

Date: 2026-05-28
Project: WebCongNghe
Scope: Storefront and CRUD/admin UI redesign

## Goal

Redesign the existing ASP.NET Core MVC UI based on `UI-Design.md` into a bright-tech storefront with a modern sales-focused landing experience, while keeping current controllers, models, routing, and CRUD behavior intact.

The redesign must also unify the CRUD/admin screens with the storefront so the entire application feels like one product. Admin pages should share the same visual language as the store, but be calmer, cleaner, and more dashboard-oriented.

## Constraints

- Keep current ASP.NET Core MVC structure.
- Do not change existing business flows or CRUD behavior.
- Reuse current data from models and controllers.
- Stay within the current Bootstrap-based frontend stack.
- Avoid introducing a separate admin application or JS-heavy client-side architecture.

## Visual Direction

### Core theme

- Bright-tech visual system.
- High-contrast light surfaces.
- Blaze Orange as the primary accent and CTA color.
- Rounded, premium card shapes and pill buttons.
- Bold, sales-oriented storefront.
- Restrained, dashboard-oriented admin surfaces.

### Tokens

- Primary text: `#0c0c0c`
- Background: `#ffffff`
- Soft background: `#f7f7f8`
- Surface: `#eff0f3`
- Accent: `#ff5f34`
- Accent support: warm orange gradient variations derived from the accent

### Typography and spacing

- Use the existing sans-serif stack with stronger hierarchy.
- Large hero heading on the home page.
- Clear section rhythm with generous spacing.
- Card radius should feel soft and premium, closer to the `UI-Design.md` direction than current Bootstrap defaults.

## Information Architecture

### Shared shell

The application will continue to use one main shared layout, but its structure and styling will be rebuilt:

- Sticky header with stronger brand treatment
- Navigation links for home, products, and categories
- Primary CTA in header
- Main content shell with layered backgrounds
- Footer redesigned into a richer multi-column marketing footer

This preserves the current app structure while making both store and admin feel intentional.

### Storefront

#### Home page

The current home page behaves like an admin dashboard. It will be rebuilt into a storefront landing page with:

- Large hero section with slogan, CTA, secondary CTA, and highlight badges
- Supporting stat chips or trust signals
- Category strip or discovery section
- Featured product grid using current product data where available
- Promotional content block
- Benefit or service section

Marketing copy may be added where needed to strengthen presentation without affecting functionality.

#### Product listing

The product index will shift from a table-heavy management page to a hybrid storefront catalog:

- Strong page intro header
- Product cards in a responsive grid
- Each card includes image, title, category, price, and actions
- Admin actions remain available, but presented more cleanly
- Empty state redesigned to feel intentional rather than placeholder-like

#### Product detail

The product detail page will become a merchandising page:

- Large image area
- Product information panel
- Price emphasis
- Category and description presented as structured metadata
- Clear primary and secondary actions

### CRUD/Admin

Admin is not a separate app in this codebase, so CRUD pages will be styled as a backoffice layer within the same shell.

#### Product admin views

- Index should read as a polished management screen while still supporting browsing
- Add and Update pages should use cleaner form layouts, grouped fields, stronger labels, and improved image preview treatment
- Display and Delete pages should use summary cards and decision-focused layouts

#### Category admin views

- Index should feel like a lightweight admin table with cleaner spacing
- Add and Update should use the same form language as product forms
- Display and Delete should mirror the product detail/delete visual structure

## Component Strategy

The redesign will rely on a consistent component layer in `site.css`:

- Shared shell components
- Hero sections
- Feature and metric cards
- Product cards
- Data tables
- Form surfaces
- Badge, chip, and pill patterns
- Empty states
- Responsive action groups

The current CSS contains legacy dark and purple styling that conflicts with the approved direction. That layer will be replaced or heavily refactored so the whole system is visually coherent.

## Responsive Behavior

- Mobile navigation remains Bootstrap collapse based
- Home hero stacks vertically on smaller screens
- Product grids adapt from 1 to 4 columns based on viewport
- CRUD action groups wrap cleanly on narrow screens
- Forms and detail layouts collapse into single-column flows on tablet/mobile

## Accessibility

- Keep visible focus states for links, buttons, and form controls
- Ensure text contrast remains strong on light surfaces
- Preserve discernible button/link text
- Keep image `alt` text behavior intact

## Implementation Plan Shape

Implementation should proceed in this order:

1. Rebuild design tokens and shared shell in `site.css` and `_Layout.cshtml`
2. Redesign the home page into the new storefront landing page
3. Redesign product listing and product detail views
4. Redesign category index/detail screens
5. Redesign product/category add, update, and delete screens
6. Run build verification and adjust visual regressions if needed

## Out of Scope

- Changing database schema
- Changing controller behavior
- Adding authentication or a real separate admin area
- Building a cart or checkout workflow
- Adding frontend frameworks beyond the current stack
