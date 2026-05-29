namespace WebCongNghe.Models;

public class HomeViewModel
{
    public HomeHeroProductViewModel? FeaturedHeroProduct { get; init; }
    public IReadOnlyList<HomeCategoryCardViewModel> FeaturedCategories { get; init; } = [];
    public IReadOnlyList<HomeProductCardViewModel> FlashSaleProducts { get; init; } = [];
}

public class HomeHeroProductViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = "/images/sample/macbook-air-m3.png";
    public decimal Price { get; init; }
    public decimal? OriginalPrice { get; init; }
    public string CategoryName { get; init; } = string.Empty;
}

public class HomeCategoryCardViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = "/images/sample/macbook-air-m3.png";
    public int ProductCount { get; init; }
}

public class HomeProductCardViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = "/images/sample/macbook-air-m3.png";
    public decimal Price { get; init; }
    public decimal? OriginalPrice { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string BadgeText { get; init; } = string.Empty;
}
