# UI Design — WebDienTu

Phiên bản: 1.0
Ngày: 2026-05-28
Tác giả: GitHub Copilot (hỗ trợ)

## Mục lục
- Tổng quan
- System tokens (màu, typography, spacing)
- Layout chung
- Header / Navigation
- Footer
- Trang chủ (Store)
- Trang sản phẩm (Product listing & detail)
- Giỏ hàng (Cart)
- Tài khoản (Account)
- Backoffice / Admin
- Components chi tiết (Buttons, Cards, Inputs, Badges)
- Responsive behavior
- Accessibility
- Assets & file references

## Tổng quan
Giao diện `WebDienTu` dùng theme "bright-tech": độ tương phản cao, nền sáng (`Canvas White`) với accent màu cam "Blaze Orange" cho các call-to-action. Kiến trúc chia thành hai khu chính: Store (frontend) và Admin (backoffice).

## System tokens (tóm tắt)
- Primary text: Midnight `#0c0c0c`.
- Backgrounds: Canvas White `#ffffff`, Light Frost `#f7f7f8`, Pale Clay `#eff0f3`.
- Accent / CTA: Blaze Orange `#ff5f34` (gradient có sẵn cho hero/khuyến mãi).
- Font chính: FontSeasonSans (substitute: Inter / system sans).
- Base spacing unit: 4px; section gap: 48px; card radius: 28px; button radius: 9999px (pill).

## Layout chung
- Header sticky (navbar) với brand bên trái, nav links ở giữa/phải, và CTA `Mua ngay` dạng `btn btn-electric`.
- Nội dung chính nằm trong `main.container.shell` (container bootstrap tùy biến).
- Footer full-width nền đen (`Ink Black`).
- Admin layout là sidebar trái cố định (`.admin-sidebar`) + content chính bên phải (`.admin-main`).

## Header / Navigation
- File: [Views/Shared/_Layout.cshtml](Views/Shared/_Layout.cshtml#L1)
- Brand: `brand-mark` gồm `brand-chip` và tên shop.
- Navigation items: `nav-link` với trạng thái active dựa trên controller hiện tại.
- Mobile: sử dụng Bootstrap collapse (hamburger) để ẩn nav.
- CTA: `a.btn.btn-electric` — style chính là pill filled với Blaze Orange.

## Footer
- File: [Views/Shared/_Layout.cshtml](Views/Shared/_Layout.cshtml#L1)
- Ba cột: giới thiệu, danh mục, hỗ trợ.
- Nền: đen, text trắng; khoảng cách các link rõ ràng.

## Trang chủ (Store)
- Hero (có thể full-bleed): lớn, headline dùng `--text-display` hoặc `--text-heading-lg`, background gradient hoặc hero image.
- Section rhythm: xen kẽ Light Frost / Canvas White.
- Blocks: featured products (card grid), categories, promotional banner, newsletter signup.

## Trang sản phẩm
- Listing: grid 3–4 cột trên desktop (bootstrap `row` + `col`), mỗi item là Product Card (ảnh + title + price + CTA).
- Product Detail: ảnh lớn trái / thông tin phải trên desktop; mobile stacked. Add-to-cart button nổi bật (Blaze Orange).

## Giỏ hàng (Cart)
- Danh sách item (thumbnail + title + qty selector + price) trong container; summary box (subtotal, shipping placeholder, checkout CTA) ở bên hoặc phía cuối trên mobile.

## Tài khoản (Account)
- Login / Register: đơn giản, form trung tâm; inputs có radius 16px, border Blaze Orange khi focus.

## Backoffice / Admin
- File: [Views/Shared/_AdminLayout.cshtml](Views/Shared/_AdminLayout.cshtml#L1)
- Sidebar trái vertical: link navigation đơn giản, highlight active.
- Topbar: search, quick actions (`btn-electric btn-sm`).
- Content: dashboard sử dụng cards metric, tables cho danh sách sản phẩm và đơn hàng.

## Components chi tiết
### Buttons
- `.btn-electric` (Primary): background `--color-blaze-orange`, color white, border-radius 9999px, padding ~12–18px; hover: slightly darker gradient.
- `.btn-ghost` (Secondary on dark): transparent + 1px white border.
- `.btn-sm` variants for admin controls.

### Product Card
- Container radius 28px, background `--surface-light-frost` hoặc transparent; image top with radius, title (FontSeasonSans 16–20px), price bold, CTA button below.

### Forms / Inputs
- Inputs: background `#ffffff`, border `1px solid var(--color-blaze-orange)` on focus, radius 16px, padding 12–20px vertical.
- Placeholders: `--color-storm-gray`.

### Badges / Chips
- Pills with radius 9999px; `.brand-chip` used for compact brand icon.

## Responsive behavior
- Breakpoints rely on Bootstrap defaults; custom tweaks:
  - Mobile nav: collapse into hamburger.
  - Product grid: 1 column (xs), 2 (sm), 3 (md), 4 (lg+).
  - Admin: sidebar collapses to top bar or overlay on small screens.

## Accessibility
- Ensure all actionable links/buttons have discernible text.
- Use `aria-label` on icon-only buttons.
- Color contrast: primary text vs background meets WCAG AA (Midnight on Canvas White). CTA text is white on Blaze Orange — verify contrast.
- Keyboard focus: visible outline for interactive elements (offset using `box-shadow` not just color change).

## Assets & file references
- Global layout: [Views/Shared/_Layout.cshtml](Views/Shared/_Layout.cshtml#L1)
- Admin layout: [Views/Shared/_AdminLayout.cshtml](Views/Shared/_AdminLayout.cshtml#L1)
- Main CSS: [wwwroot/css/site.css](wwwroot/css/site.css#L1)
- Scripts: [wwwroot/js/site.js](wwwroot/js/site.js)

## Implementation notes
- Prefer CSS variables (see `DESIGN.md` tokens) for colors/spacing.
- Centralize component styles in `site.css` or a new `components.css` and import in `_Layout.cshtml`.
- Keep admin styles scoped under `.admin-body` to avoid leakage.

---

Nếu bạn muốn, mình có thể:
- Bổ sung sơ đồ wireframe bằng ASCII/mermaid,
- Tạo `components.css` mẫu với các class chính (`.btn-electric`, `.product-card`, `.brand-chip`),
- Hoặc cập nhật `wwwroot/css/site.css` trực tiếp với token CSS variables.

File đã tạo: [WebDienTu/UI-Design.md](WebDienTu/UI-Design.md)
