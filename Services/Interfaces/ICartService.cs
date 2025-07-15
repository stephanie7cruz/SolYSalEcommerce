using SolYSalEcommerce.DTOs.Cart;
using SolYSalEcommerce.Models; 

namespace SolYSalEcommerce.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartItemsByUserId(Guid userId);
        Task<CartItem?> AddToCart(Guid userId, AddToCartDto addToCartDto);
        Task<CartItem?> UpdateCartItemQuantity(Guid userId, UpdateCartItemDto updateCartItemDto);
        Task<bool> RemoveCartItem(Guid userId, Guid productVariantId);
        Task<bool> ClearCart(Guid userId);
    }
}