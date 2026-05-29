namespace WebCongNghe.Models;

public class CartViewModel
{
    public IReadOnlyList<CartItemViewModel> Items { get; init; } = [];
    public int TotalQuantity { get; init; }
    public decimal Subtotal { get; init; }
    public decimal EstimatedShipping { get; init; }
    public decimal GrandTotal => Subtotal + EstimatedShipping;
    public bool IsEmpty => Items.Count == 0;
}

public class CartItemViewModel
{
    public int CartItemId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal => UnitPrice * Quantity;
}
