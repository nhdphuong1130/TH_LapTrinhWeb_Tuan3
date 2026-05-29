# WebCongNghe Lab 3 UI Redesign Design

**Context**

Project hiện tại là ứng dụng ASP.NET Core MVC cho bài thực hành Lab 3, đã có:
- SQL Server LocalDB
- Entity Framework Core
- CRUD cho `Product` và `Category`
- upload và preview ảnh sản phẩm

Mục tiêu của lần thay đổi này là thiết kế lại giao diện để ứng dụng nhìn hiện đại hơn, lấy cảm hứng từ mẫu portfolio dark/purple mà người dùng cung cấp, nhưng phải phù hợp với một ứng dụng quản trị CRUD.

## Goals

- Nâng cấp toàn bộ UI để nhìn hiện đại, nhất quán và có cá tính hơn.
- Giữ nguyên luồng nghiệp vụ, controller, repository và database hiện tại.
- Tối ưu giao diện cho tác vụ quản trị: xem danh sách, nhập form, sửa dữ liệu, xem chi tiết, xác nhận xóa.
- Giữ ứng dụng dễ implement bằng Razor Views + Bootstrap hiện có.

## Non-Goals

- Không biến ứng dụng thành landing page hoặc portfolio site.
- Không thêm animation nặng như particle canvas, 3D canvas, orb background, case-study modal.
- Không đổi cấu trúc backend trừ khi cần bổ sung rất nhỏ để phục vụ hiển thị.

## Design Direction

Áp dụng tinh thần của mẫu tham chiếu theo cách tiết chế:

- nền tối tím-xám
- accent tím sáng và hồng nhạt
- khối giao diện có chiều sâu nhẹ bằng border sáng mờ và blur vừa phải
- typography sang hơn ở heading, nhưng phần dữ liệu vẫn ưu tiên readability
- hover/focus state rõ ràng cho button, nav và form controls

Giao diện phải đọc giống một admin dashboard thực dụng, không phải một trang giới thiệu cá nhân.

## Information Architecture

### Global Layout

- Header cố định phía trên
- Brand: `WebCongNghe Lab 3`
- Menu chính:
  - Trang chu
  - San pham
  - Danh muc
- Khu nội dung chính rộng, canh giữa
- Footer tối giản

### Home

Trang chủ sẽ là dashboard nhỏ, không phải hero kéo dài.

Thành phần:
- tiêu đề giới thiệu app
- mô tả ngắn về stack và tính năng
- 2 CTA chính:
  - Quan ly san pham
  - Quan ly danh muc
- 1 vùng trạng thái/tóm tắt:
  - EF Core
  - SQL Server LocalDB
  - CRUD
  - Upload preview

### Product List

Thành phần:
- page header
- nút `Them san pham`
- bảng sản phẩm
- cột:
  - ảnh
  - tên
  - giá
  - danh mục
  - tác vụ
- nút tác vụ:
  - Xem
  - Sua
  - Xoa

Thiết kế ưu tiên scan nhanh và thao tác lặp.

### Product Add / Update

Thành phần:
- form 2 cột trên desktop
- cột trái: dữ liệu chính
- cột phải: khung preview ảnh / hướng dẫn ngắn
- field:
  - tên sản phẩm
  - giá
  - mô tả
  - danh mục
  - file ảnh
- validation rõ ràng

### Product Display

Thành phần:
- ảnh lớn vừa phải
- thông tin chính theo khối
- các nút:
  - Sua
  - Xoa
  - Danh sach

### Product Delete

Thành phần:
- cảnh báo xác nhận
- panel tóm tắt sản phẩm
- hành động:
  - Xoa
  - Huy

### Category Pages

Giữ cùng design system với Product nhưng đơn giản hơn:
- list page là bảng gọn
- add/update là form hẹp
- display/delete là panel xác nhận hoặc thông tin tối giản

## Visual System

### Colors

- page background: tím đen / xám tím rất đậm
- primary surface: tím-xám đậm
- secondary surface: tím đậm hơn hoặc gần đen
- primary accent: tím sáng
- supporting accent: hồng nhạt
- text: trắng ngà
- muted text: xám tím nhạt
- danger: đỏ hồng đủ rõ nhưng không gắt

### Components

- card radius tối đa 8px
- border mảnh sáng nhẹ
- button đặc cho primary, outline/tinted cho secondary
- table row hover nhẹ
- input nền tối, viền sáng mờ, focus ring accent
- thumbnail và preview dùng kích thước cố định, không nhảy layout

## Responsive Behavior

- desktop/laptop là ưu tiên chính
- tablet/mobile vẫn đọc được
- navbar co gọn theo Bootstrap
- bảng có thể cuộn ngang khi cần
- form 2 cột chuyển về 1 cột trên màn hình hẹp

## Implementation Scope

Sẽ chỉnh các file sau:
- `Views/Shared/_Layout.cshtml`
- `wwwroot/css/site.css`
- `Views/Home/Index.cshtml`
- `Views/Product/*.cshtml`
- `Views/Categories/*.cshtml`

Không đổi:
- controller logic hiện tại
- repository
- migrations
- cấu hình database

## Verification

Sau khi implement cần xác minh:
- `dotnet build` pass
- `dotnet test` pass
- chạy app local thành công
- các route chính trả về HTTP 200:
  - `/`
  - `/Product`
  - `/Categories`

