using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SolYSalEcommerce.Data;
using SolYSalEcommerce.DTOs.Cart;
using SolYSalEcommerce.Models;
using SolYSalEcommerce.Services.Interfaces;

namespace SolYSalEcommerce.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartItemsByUserId(Guid userId)
        {
            return await _context.CartItems
                                 .Include(ci => ci.ProductVariant)
                                     .ThenInclude(pv => pv.Product)
                                 .Where(ci => ci.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<CartItem?> AddToCart(Guid userId, AddToCartDto addToCartDto)
        {
            var productVariant = await _context.ProductVariants.FindAsync(addToCartDto.ProductVariantId);
            if (productVariant == null || productVariant.Stock < addToCartDto.Quantity)
            {
                return null;
            }

            var cartItem = await _context.CartItems
                                         .SingleOrDefaultAsync(ci => ci.UserId == userId && ci.ProductVariantId == addToCartDto.ProductVariantId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    UserId = userId,
                    ProductVariantId = addToCartDto.ProductVariantId,
                    Quantity = addToCartDto.Quantity,
                    DateAdded = DateTime.UtcNow
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                if (productVariant.Stock < cartItem.Quantity + addToCartDto.Quantity)
                {
                    return null;
                }
                cartItem.Quantity += addToCartDto.Quantity;
            }

            productVariant.Stock -= addToCartDto.Quantity;

            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem?> UpdateCartItemQuantity(Guid userId, UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _context.CartItems
                                         .Include(ci => ci.ProductVariant)
                                         .SingleOrDefaultAsync(ci => ci.UserId == userId && ci.ProductVariantId == updateCartItemDto.ProductVariantId);

            if (cartItem == null)
            {
                return null;
            }

            var oldQuantity = cartItem.Quantity;
            var newQuantity = updateCartItemDto.Quantity;

            var stockChange = newQuantity - oldQuantity;

            if (newQuantity == 0)
            {
                _context.CartItems.Remove(cartItem);
                cartItem.ProductVariant.Stock += oldQuantity;
            }
            else
            {
                if (cartItem.ProductVariant.Stock < stockChange && stockChange > 0)
                {
                    return null;
                }
                cartItem.Quantity = newQuantity;
                cartItem.ProductVariant.Stock -= stockChange;
            }

            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> RemoveCartItem(Guid userId, Guid productVariantId)
        {
            var cartItem = await _context.CartItems
                                         .Include(ci => ci.ProductVariant)
                                         .SingleOrDefaultAsync(ci => ci.UserId == userId && ci.ProductVariantId == productVariantId);

            if (cartItem == null)
            {
                return false;
            }

            _context.CartItems.Remove(cartItem);
            cartItem.ProductVariant.Stock += cartItem.Quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            var cartItems = await _context.CartItems.Where(ci => ci.UserId == userId)
                                                 .Include(ci => ci.ProductVariant)
                                                 .ToListAsync();
            if (!cartItems.Any())
            {
                return false;
            }

            foreach (var item in cartItems)
            {
                item.ProductVariant.Stock += item.Quantity;
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}