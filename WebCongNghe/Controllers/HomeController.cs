using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebCongNghe.Models;
using WebCongNghe.Repositories;

namespace WebCongNghe.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = (await _productRepository.GetAllAsync()).ToList();
            var categories = (await _categoryRepository.GetAllAsync()).ToList();

            var heroSource = products
                .Where(product => !string.IsNullOrWhiteSpace(product.ImageUrl))
                .OrderByDescending(product => product.Price)
                .FirstOrDefault();

            var featuredCategories = categories
                .Select(category =>
                {
                    var categoryProducts = products.Where(product => product.CategoryId == category.Id).ToList();
                    var coverProduct = categoryProducts.FirstOrDefault(product => !string.IsNullOrWhiteSpace(product.ImageUrl))
                        ?? categoryProducts.FirstOrDefault();

                    return new HomeCategoryCardViewModel
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = BuildCategoryDescription(category.Name),
                        ImageUrl = coverProduct?.ImageUrl ?? "/images/sample/macbook-air-m3.png",
                        ProductCount = categoryProducts.Count
                    };
                })
                .Where(category => category.ProductCount > 0)
                .Take(4)
                .ToList();

            var flashSaleProducts = products
                .Where(product => !string.IsNullOrWhiteSpace(product.ImageUrl))
                .OrderByDescending(product => product.Price)
                .Take(4)
                .Select(product => new HomeProductCardViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = string.IsNullOrWhiteSpace(product.Description)
                        ? "Sản phẩm cong nghe noi bat trong catalog hien tai."
                        : product.Description,
                    ImageUrl = product.ImageUrl ?? "/images/sample/macbook-air-m3.png",
                    Price = product.Price,
                    OriginalPrice = Math.Round(product.Price * 1.08m, 0),
                    CategoryName = product.Category?.Name ?? "Sản phẩm",
                    BadgeText = BuildBadgeText(product.Category?.Name)
                })
                .ToList();

            var viewModel = new HomeViewModel
            {
                FeaturedHeroProduct = heroSource is null
                    ? null
                    : new HomeHeroProductViewModel
                    {
                        Id = heroSource.Id,
                        Name = heroSource.Name,
                        Description = string.IsNullOrWhiteSpace(heroSource.Description)
                            ? "Sản phẩm noi bat voi hieu nang va hinh anh phu hop cho hero storefront."
                            : heroSource.Description,
                        ImageUrl = heroSource.ImageUrl ?? "/images/sample/macbook-air-m3.png",
                        Price = heroSource.Price,
                        OriginalPrice = Math.Round(heroSource.Price * 1.08m, 0),
                        CategoryName = heroSource.Category?.Name ?? "Sản phẩm"
                    },
                FeaturedCategories = featuredCategories,
                FlashSaleProducts = flashSaleProducts
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static string BuildCategoryDescription(string categoryName) => categoryName switch
        {
            "Laptop" => "Hieu nang do hoa, di dong va lam viec cao cap.",
            "Điện thoại" => "Flagship, camera va trai nghiem mobile hien dai.",
            "Phụ kiện" => "Setup, am thanh va phu kien ho tro moi workspace.",
            _ => "Danh mục cong nghe noi bat trong storefront hien tai."
        };

        private static string BuildBadgeText(string? categoryName) => categoryName switch
        {
            "Laptop" => "Performance",
            "Điện thoại" => "Flagship",
            "Phụ kiện" => "Setup",
            _ => "Featured"
        };
    }
}
