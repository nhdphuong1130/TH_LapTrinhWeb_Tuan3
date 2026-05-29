using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCongNghe.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1001, 1, "Laptop gaming cau hinh cao, phu hop demo danh muc laptop hieu nang.", "/images/sample/laptop-gaming-asus-rog.png", "Laptop Gaming ASUS ROG", 32990000m },
                    { 1002, 1, "Laptop nhe, thiet ke gon va phu hop nhom nguoi dung van phong, hoc tap.", "/images/sample/macbook-air-m3.png", "MacBook Air M3", 28990000m },
                    { 1003, 3, "Dien thoai cao cap voi camera dep va giao dien hien dai cho demo.", "/images/sample/iphone-15.png", "iPhone 15", 21990000m },
                    { 1004, 3, "Dien thoai Android flagship, phu hop de demo nhom san pham di dong.", "/images/sample/samsung-galaxy-s24.png", "Samsung Galaxy S24", 20990000m },
                    { 1005, 2, "Chuot khong day cao cap, thao tac tot cho nhu cau van phong va sang tao.", "/images/sample/logitech-mx-master.png", "Chuot Logitech MX Master", 2490000m },
                    { 1006, 2, "Ban phim co ket noi da che do, hop voi bo san pham phu kien may tinh.", "/images/sample/keychron-keyboard.png", "Ban phim co Keychron", 2790000m }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ProductId", "Url" },
                values: new object[,]
                {
                    { 2001, 1001, "/images/sample/laptop-gaming-asus-rog.png" },
                    { 2002, 1002, "/images/sample/macbook-air-m3.png" },
                    { 2003, 1003, "/images/sample/iphone-15.png" },
                    { 2004, 1004, "/images/sample/samsung-galaxy-s24.png" },
                    { 2005, 1005, "/images/sample/logitech-mx-master.png" },
                    { 2006, 1006, "/images/sample/keychron-keyboard.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2003);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2004);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2005);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2006);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1006);
        }
    }
}
