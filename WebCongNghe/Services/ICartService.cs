using WebCongNghe.Models;

namespace WebCongNghe.Services;

public interface ICartService
{
    Task<int> GetCartItemCountAsync();
    Task<CartViewModel> GetCartAsync();
    Task AddToCartAsync(int productId, int quantity);
    Task UpdateQuantityAsync(int cartItemId, int quantity);
    Task RemoveItemAsync(int cartItemId);
    Task ClearCartAsync();
}
