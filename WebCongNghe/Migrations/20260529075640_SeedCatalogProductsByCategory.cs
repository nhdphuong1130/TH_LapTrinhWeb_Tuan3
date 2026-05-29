using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebCongNghe.Migrations
{
    /// <inheritdoc />
    public partial class SeedCatalogProductsByCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM [Products] WHERE [Id] = 1007)
                BEGIN
                    UPDATE [Products]
                    SET [CategoryId] = 3,
                        [Description] = N'Phien ban cao cap voi cum camera lon, phu hop demo nhom dien thoai flagship cua Apple.',
                        [ImageUrl] = N'/images/494a097644004c28a34bed10ae03a4ec.png',
                        [Name] = N'iPhone 15 Pro Max',
                        [Price] = 29990000.0
                    WHERE [Id] = 1007;
                END
                ELSE
                BEGIN
                    SET IDENTITY_INSERT [Products] ON;
                    INSERT INTO [Products] ([Id], [CategoryId], [Description], [ImageUrl], [Name], [Price])
                    VALUES (1007, 3, N'Phien ban cao cap voi cum camera lon, phu hop demo nhom dien thoai flagship cua Apple.', N'/images/494a097644004c28a34bed10ae03a4ec.png', N'iPhone 15 Pro Max', 29990000.0);
                    SET IDENTITY_INSERT [Products] OFF;
                END
                """);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1008, 1, "Laptop man hinh OLED cao cap, phu hop doanh nhan va nha sang tao can hieu nang on dinh.", "/images/1.png", "Dell XPS 15 OLED", 45500000m },
                    { 1009, 1, "ThinkPad mong nhe, ben bi, phu hop moi truong van phong va quan ly du an di dong.", "/images/unnamed (2).png", "Lenovo ThinkPad X1 Carbon Gen 11", 39900000m },
                    { 1010, 1, "Laptop cao cap thiet ke linh hoat, man hinh dep va phu hop nhu cau hoc tap, thuyet trinh, sang tao.", "/images/unnamed (31).png", "HP Spectre x360 14", 36990000m },
                    { 1011, 1, "Thiet bi 2-trong-1 gon nhe, ket hop ban phim roi de phuc vu hoc tap, ghi chu va lam viec co dong.", "/images/unnamed (35).png", "Surface Pro 9 Keyboard Bundle", 31990000m },
                    { 1012, 2, "Tai nghe over-ear chong on tot, hop cho nguoi dung di chuyen nhieu va can su tap trung.", "/images/70a9f09bf5634f80a9578de40ac7fa77.png", "Tai nghe Sony WH-1000XM5", 7990000m },
                    { 1013, 2, "Chuot gaming do tre thap, form gon va phu hop setup hieu nang cao.", "/images/b08427a925554faeb29c454a04004dc2.png", "Chuot gaming Razer Cobra Pro", 3290000m },
                    { 1014, 2, "Cu sac cong suat lon di kem cap boc du, phu hop laptop, tablet va dien thoai dong cong suat cao.", "/images/unnamed (22).png", "Bo sac nhanh USB-C 100W", 1190000m },
                    { 1015, 2, "Tai nghe khong day thiet ke toi gian, am thanh can bang va phu hop cho game thu, streamer.", "/images/unnamed (11).png", "Tai nghe SteelSeries Nova", 4590000m },
                    { 1016, 3, "Dien thoai man hinh lon, camera nhieu ong kinh va hieu nang cao cho nhom nguoi dung nang dong.", "/images/unnamed (24).png", "Xiaomi 14 Ultra", 25990000m },
                    { 1017, 3, "Dien thoai thiet ke bat mat, man hinh sang va phu hop nhu cau chup anh, giai tri hang ngay.", "/images/unnamed (25).png", "OPPO Reno11 Pro", 16990000m },
                    { 1018, 3, "Flagship Android giao dien sach, camera thong minh va trai nghiem phan mem muot ma.", "/images/unnamed (26).png", "Google Pixel 8 Pro", 23990000m }
                });

            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM [ProductImages] WHERE [Id] = 2007)
                BEGIN
                    UPDATE [ProductImages]
                    SET [ProductId] = 1007,
                        [Url] = N'/images/494a097644004c28a34bed10ae03a4ec.png'
                    WHERE [Id] = 2007;
                END
                ELSE
                BEGIN
                    SET IDENTITY_INSERT [ProductImages] ON;
                    INSERT INTO [ProductImages] ([Id], [ProductId], [Url])
                    VALUES (2007, 1007, N'/images/494a097644004c28a34bed10ae03a4ec.png');
                    SET IDENTITY_INSERT [ProductImages] OFF;
                END
                """);

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ProductId", "Url" },
                values: new object[,]
                {
                    { 2008, 1008, "/images/1.png" },
                    { 2009, 1009, "/images/unnamed (2).png" },
                    { 2010, 1010, "/images/unnamed (31).png" },
                    { 2011, 1011, "/images/unnamed (35).png" },
                    { 2012, 1012, "/images/70a9f09bf5634f80a9578de40ac7fa77.png" },
                    { 2013, 1013, "/images/b08427a925554faeb29c454a04004dc2.png" },
                    { 2014, 1014, "/images/unnamed (22).png" },
                    { 2015, 1015, "/images/unnamed (11).png" },
                    { 2016, 1016, "/images/unnamed (24).png" },
                    { 2017, 1017, "/images/unnamed (25).png" },
                    { 2018, 1018, "/images/unnamed (26).png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2007);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2008);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2009);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2010);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2011);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2012);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2013);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2014);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2015);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2016);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2017);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2018);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1018);
        }
    }
}
