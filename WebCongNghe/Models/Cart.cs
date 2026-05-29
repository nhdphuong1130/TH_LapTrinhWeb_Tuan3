namespace WebCongNghe.Models;

public class Cart
{
    public int Id { get; set; }
    public string CartCode { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public List<CartItem> Items { get; set; } = [];
}
