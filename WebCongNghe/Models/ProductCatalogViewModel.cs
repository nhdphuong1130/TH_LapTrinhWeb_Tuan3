namespace WebCongNghe.Models;

public class ProductCatalogViewModel
{
    public IReadOnlyList<ProductCatalogCardViewModel> Products { get; init; } = [];
    public IReadOnlyList<CatalogFilterOptionViewModel> Categories { get; init; } = [];
    public IReadOnlyList<CatalogFilterOptionViewModel> Brands { get; init; } = [];
    public IReadOnlyList<string> CpuOptions { get; init; } = [];
    public IReadOnlyList<string> RamOptions { get; init; } = [];
    public string PageTitle { get; init; } = "Sản phẩm cong nghe";
    public string PageDescription { get; init; } = string.Empty;
    public string? CurrentCategoryName { get; init; }
    public int? SelectedCategoryId { get; init; }
    public IReadOnlyList<string> SelectedBrands { get; init; } = [];
    public string? SelectedCpu { get; init; }
    public string? SelectedRam { get; init; }
    public decimal? SelectedMinPrice { get; init; }
    public decimal? SelectedMaxPrice { get; init; }
    public string Sort { get; init; } = "newest";
    public int Page { get; init; } = 1;
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public int StartItem { get; init; }
    public int EndItem { get; init; }
    public bool HasFiltersApplied =>
        SelectedCategoryId.HasValue ||
        SelectedBrands.Count > 0 ||
        !string.IsNullOrWhiteSpace(SelectedCpu) ||
        !string.IsNullOrWhiteSpace(SelectedRam) ||
        SelectedMinPrice.HasValue ||
        SelectedMaxPrice.HasValue;
}

public class ProductCatalogCardViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public decimal? OriginalPrice { get; init; }
    public string? ImageUrl { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Cpu { get; init; } = string.Empty;
    public string Ram { get; init; } = string.Empty;
    public decimal Rating { get; init; }
    public int ReviewCount { get; init; }
    public string BadgeText { get; init; } = string.Empty;
    public string BadgeTone { get; init; } = "default";
}

public class CatalogFilterOptionViewModel
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public int Count { get; init; }
    public bool IsSelected { get; init; }
}
