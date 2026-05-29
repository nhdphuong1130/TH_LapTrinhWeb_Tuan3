using Microsoft.EntityFrameworkCore;
using WebCongNghe.Data;
using WebCongNghe.Models;

namespace WebCongNghe.Services;

public class CartService : ICartService
{
    private const string CartCookieName = "webcongnghe_cart";
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TimeProvider _timeProvider;

    public CartService(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        TimeProvider timeProvider)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _timeProvider = timeProvider;
    }

    public async Task<int> GetCartItemCountAsync()
    {
        var cart = await GetCartEntityAsync(createIfMissing: false);
        return cart?.Items.Sum(item => item.Quantity) ?? 0;
    }

    public async Task<CartViewModel> GetCartAsync()
    {
        var cart = await GetCartEntityAsync(createIfMissing: false);
        if (cart is null)
        {
            return new CartViewModel();
        }

        var items = cart.Items
            .OrderBy(item => item.CreatedAtUtc)
            .Select(item => new CartItemViewModel
            {
                CartItemId = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? "San pham",
                CategoryName = item.Product?.Category?.Name ?? "Danh muc",
                ImageUrl = item.Product?.ImageUrl,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            })
            .ToList();

        var subtotal = items.Sum(item => item.LineTotal);
        var shipping = items.Count == 0 ? 0m : 30000m;

        return new CartViewModel
        {
            Items = items,
            TotalQuantity = items.Sum(item => item.Quantity),
            Subtotal = subtotal,
            EstimatedShipping = shipping
        };
    }

    public async Task AddToCartAsync(int productId, int quantity)
    {
        if (quantity <= 0)
        {
            quantity = 1;
        }

        var product = await _context.Products.FirstOrDefaultAsync(productItem => productItem.Id == productId);
        if (product is null)
        {
            return;
        }

        var cart = await GetCartEntityAsync(createIfMissing: true);
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var existingItem = cart!.Items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem is null)
        {
            existingItem = new CartItem
            {
                ProductId = product.Id,
                Quantity = quantity,
                UnitPrice = product.Price,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };
            cart.Items.Add(existingItem);
        }
        else
        {
            existingItem.Quantity += quantity;
            existingItem.UnitPrice = product.Price;
            existingItem.UpdatedAtUtc = now;
        }

        cart.UpdatedAtUtc = now;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int cartItemId, int quantity)
    {
        var cart = await GetCartEntityAsync(createIfMissing: false);
        if (cart is null)
        {
            return;
        }

        var item = cart.Items.FirstOrDefault(cartItem => cartItem.Id == cartItemId);
        if (item is null)
        {
            return;
        }

        if (quantity <= 0)
        {
            _context.CartItems.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
            item.UpdatedAtUtc = _timeProvider.GetUtcNow().UtcDateTime;
        }

        cart.UpdatedAtUtc = _timeProvider.GetUtcNow().UtcDateTime;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(int cartItemId)
    {
        var cart = await GetCartEntityAsync(createIfMissing: false);
        if (cart is null)
        {
            return;
        }

        var item = cart.Items.FirstOrDefault(cartItem => cartItem.Id == cartItemId);
        if (item is null)
        {
            return;
        }

        _context.CartItems.Remove(item);
        cart.UpdatedAtUtc = _timeProvider.GetUtcNow().UtcDateTime;
        await _context.SaveChangesAsync();
    }

    public async Task ClearCartAsync()
    {
        var cart = await GetCartEntityAsync(createIfMissing: false);
        if (cart is null || cart.Items.Count == 0)
        {
            return;
        }

        _context.CartItems.RemoveRange(cart.Items);
        cart.UpdatedAtUtc = _timeProvider.GetUtcNow().UtcDateTime;
        await _context.SaveChangesAsync();
    }

    private async Task<Cart?> GetCartEntityAsync(bool createIfMissing)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return null;
        }

        var cartCode = httpContext.Request.Cookies[CartCookieName];
        Cart? cart = null;

        if (!string.IsNullOrWhiteSpace(cartCode))
        {
            cart = await _context.Carts
                .Include(existingCart => existingCart.Items)
                .ThenInclude(item => item.Product)
                .ThenInclude(product => product!.Category)
                .FirstOrDefaultAsync(existingCart => existingCart.CartCode == cartCode);
        }

        if (cart is not null || !createIfMissing)
        {
            return cart;
        }

        var now = _timeProvider.GetUtcNow().UtcDateTime;
        cartCode = Guid.NewGuid().ToString("N");
        cart = new Cart
        {
            CartCode = cartCode,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();

        httpContext.Response.Cookies.Append(
            CartCookieName,
            cartCode,
            new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Expires = now.AddDays(30)
            });

        return cart;
    }
}
