namespace WebCongNghe.Models;

public class CategoryManagementViewModel
{
    public List<CategoryManagementCategoryItemViewModel> Categories { get; init; } = [];
    public Category? SelectedCategory { get; init; }
    public List<Product> Products { get; init; } = [];
    public int TotalCategories => Categories.Count;
    public int SelectedProductCount => Products.Count;
    public bool HasCategories => Categories.Count > 0;
}

public class CategoryManagementCategoryItemViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsSelected { get; init; }
    public int ProductCount { get; init; }
}
