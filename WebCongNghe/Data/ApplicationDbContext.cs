using Microsoft.EntityFrameworkCore;
using WebCongNghe.Models;

namespace WebCongNghe.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Cart>()
            .HasIndex(cart => cart.CartCode)
            .IsUnique();

        modelBuilder.Entity<CartItem>()
            .Property(item => item.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CartItem>()
            .HasOne(item => item.Cart)
            .WithMany(cart => cart.Items)
            .HasForeignKey(item => item.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasData(
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Phụ kiện" },
                new Category { Id = 3, Name = "Điện thoại" });

        modelBuilder.Entity<Product>()
            .HasData(
                new Product
                {
                    Id = 1001,
                    Name = "Laptop Gaming ASUS ROG",
                    Price = 32990000m,
                    Description = "Laptop gaming cấu hình cao, phù hợp demo danh mục laptop hiệu năng.",
                    ImageUrl = "/images/sample/laptop-gaming-asus-rog.png",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 1002,
                    Name = "MacBook Air M3",
                    Price = 28990000m,
                    Description = "Laptop nhẹ, thiết kế gọn và phù hợp nhóm người dùng văn phòng, học tập.",
                    ImageUrl = "/images/sample/macbook-air-m3.png",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 1008,
                    Name = "Dell XPS 15 OLED",
                    Price = 45500000m,
                    Description = "Laptop màn hình OLED cao cấp, phù hợp doanh nhân và nhà sáng tạo cần hiệu năng ổn định.",
                    ImageUrl = "/images/1.png",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 1009,
                    Name = "Lenovo ThinkPad X1 Carbon Gen 11",
                    Price = 39900000m,
                    Description = "ThinkPad mỏng nhẹ, bền bỉ, phù hợp môi trường văn phòng và quản lý dự án di động.",
                    ImageUrl = "/images/unnamed (2).png",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 1010,
                    Name = "HP Spectre x360 14",
                    Price = 36990000m,
                    Description = "Laptop cao cấp thiết kế linh hoạt, màn hình đẹp và phù hợp nhu cầu học tập, thuyết trình, sáng tạo.",
                    ImageUrl = "/images/unnamed (31).png",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 1011,
                    Name = "Surface Pro 9 Keyboard Bundle",
                    Price = 31990000m,
                    Description = "Thiết bị 2-trong-1 gọn nhẹ, kết hợp bàn phím rời để phục vụ học tập, ghi chú và làm việc cơ động.",
                    ImageUrl = "/images/unnamed (35).png",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 1003,
                    Name = "iPhone 15",
                    Price = 21990000m,
                    Description = "Điện thoại cao cap voi camera dep va giao dien hien dai cho demo.",
                    ImageUrl = "/images/sample/iphone-15.png",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 1004,
                    Name = "Samsung Galaxy S24",
                    Price = 20990000m,
                    Description = "Điện thoại Android flagship, phu hop de demo nhom san pham di dong.",
                    ImageUrl = "/images/sample/samsung-galaxy-s24.png",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 1007,
                    Name = "iPhone 15 Pro Max",
                    Price = 29990000m,
                    Description = "Phiên bản cao cấp với cụm camera lớn, phù hợp demo nhóm điện thoại flagship của Apple.",
                    ImageUrl = "/images/494a097644004c28a34bed10ae03a4ec.png",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 1016,
                    Name = "Xiaomi 14 Ultra",
                    Price = 25990000m,
                    Description = "Điện thoại man hinh lon, camera nhieu ong kinh va hieu nang cao cho nhom nguoi dung nang dong.",
                    ImageUrl = "/images/unnamed (24).png",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 1017,
                    Name = "OPPO Reno11 Pro",
                    Price = 16990000m,
                    Description = "Điện thoại thiet ke bat mat, man hinh sang va phu hop nhu cau chup anh, giai tri hang ngay.",
                    ImageUrl = "/images/unnamed (25).png",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 1018,
                    Name = "Google Pixel 8 Pro",
                    Price = 23990000m,
                    Description = "Flagship Android giao diện sạch, camera thông minh và trải nghiệm phần mềm mượt mà.",
                    ImageUrl = "/images/unnamed (26).png",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 1005,
                    Name = "Chuột Logitech MX Master",
                    Price = 2490000m,
                    Description = "Chuột không dây cao cấp, thao tác tốt cho nhu cầu văn phòng và sáng tạo.",
                    ImageUrl = "/images/sample/logitech-mx-master.png",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 1006,
                    Name = "Bàn phím cơ Keychron",
                    Price = 2790000m,
                    Description = "Bàn phím cơ kết nối đa chế độ, hợp với bộ sản phẩm phụ kiện máy tính.",
                    ImageUrl = "/images/sample/keychron-keyboard.png",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 1012,
                    Name = "Tai nghe Sony WH-1000XM5",
                    Price = 7990000m,
                    Description = "Tai nghe over-ear chống ồn tốt, hợp cho người dùng di chuyển nhiều và cần sự tập trung.",
                    ImageUrl = "/images/70a9f09bf5634f80a9578de40ac7fa77.png",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 1013,
                    Name = "Chuột gaming Razer Cobra Pro",
                    Price = 3290000m,
                    Description = "Chuột gaming độ trễ thấp, form gọn và phù hợp setup hiệu năng cao.",
                    ImageUrl = "/images/b08427a925554faeb29c454a04004dc2.png",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 1014,
                    Name = "Bộ sạc nhanh USB-C 100W",
                    Price = 1190000m,
                    Description = "Củ sạc công suất lớn đi kèm cáp bọc dù, phù hợp laptop, tablet và điện thoại dùng công suất cao.",
                    ImageUrl = "/images/unnamed (22).png",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 1015,
                    Name = "Tai nghe SteelSeries Nova",
                    Price = 4590000m,
                    Description = "Tai nghe không dây thiết kế tối giản, âm thanh cân bằng và phù hợp cho game thủ, streamer.",
                    ImageUrl = "/images/unnamed (11).png",
                    CategoryId = 2
                });

        modelBuilder.Entity<ProductImage>()
            .HasData(
                new ProductImage { Id = 2001, ProductId = 1001, Url = "/images/sample/laptop-gaming-asus-rog.png" },
                new ProductImage { Id = 2002, ProductId = 1002, Url = "/images/sample/macbook-air-m3.png" },
                new ProductImage { Id = 2003, ProductId = 1003, Url = "/images/sample/iphone-15.png" },
                new ProductImage { Id = 2004, ProductId = 1004, Url = "/images/sample/samsung-galaxy-s24.png" },
                new ProductImage { Id = 2005, ProductId = 1005, Url = "/images/sample/logitech-mx-master.png" },
                new ProductImage { Id = 2006, ProductId = 1006, Url = "/images/sample/keychron-keyboard.png" },
                new ProductImage { Id = 2007, ProductId = 1007, Url = "/images/494a097644004c28a34bed10ae03a4ec.png" },
                new ProductImage { Id = 2008, ProductId = 1008, Url = "/images/1.png" },
                new ProductImage { Id = 2009, ProductId = 1009, Url = "/images/unnamed (2).png" },
                new ProductImage { Id = 2010, ProductId = 1010, Url = "/images/unnamed (31).png" },
                new ProductImage { Id = 2011, ProductId = 1011, Url = "/images/unnamed (35).png" },
                new ProductImage { Id = 2012, ProductId = 1012, Url = "/images/70a9f09bf5634f80a9578de40ac7fa77.png" },
                new ProductImage { Id = 2013, ProductId = 1013, Url = "/images/b08427a925554faeb29c454a04004dc2.png" },
                new ProductImage { Id = 2014, ProductId = 1014, Url = "/images/unnamed (22).png" },
                new ProductImage { Id = 2015, ProductId = 1015, Url = "/images/unnamed (11).png" },
                new ProductImage { Id = 2016, ProductId = 1016, Url = "/images/unnamed (24).png" },
                new ProductImage { Id = 2017, ProductId = 1017, Url = "/images/unnamed (25).png" },
                new ProductImage { Id = 2018, ProductId = 1018, Url = "/images/unnamed (26).png" });
    }
}
