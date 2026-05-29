using System.ComponentModel.DataAnnotations;

namespace WebCongNghe.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
    [StringLength(100)]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 100000000, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
    [Display(Name = "Gia")]
    public decimal Price { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Ảnh đại diện")]
    public string? ImageUrl { get; set; }

    public List<ProductImage>? Images { get; set; }

    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
