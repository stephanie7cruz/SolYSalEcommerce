using Microsoft.EntityFrameworkCore;
using SolYSalEcommerce.Data;
using SolYSalEcommerce.DTOs.Products;
using SolYSalEcommerce.Models;
using SolYSalEcommerce.Services.Interfaces; 

namespace SolYSalEcommerce.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _context.Products
                                         .Include(p => p.Variants)
                                         .Where(p => p.IsActive)
                                         .ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                IsActive = p.IsActive,
                Variants = p.Variants.Select(pv => new ProductVariantDto
                {
                    Id = pv.Id,
                    ProductId = pv.ProductId,
                    Color = pv.Color,
                    Size = pv.Size,
                    SKU = pv.SKU,
                    Stock = pv.Stock,
                    ImageUrl = pv.ImageUrl
                }).ToList()
            });
        }

        public async Task<ProductDto?> GetProductById(Guid id)
        {
            var product = await _context.Products
                                         .Include(p => p.Variants)
                                         .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                IsActive = product.IsActive,
                Variants = product.Variants.Select(pv => new ProductVariantDto
                {
                    Id = pv.Id,
                    ProductId = pv.ProductId,
                    Color = pv.Color,
                    Size = pv.Size,
                    SKU = pv.SKU,
                    Stock = pv.Stock,
                    ImageUrl = pv.ImageUrl
                }).ToList()
            };
        }

        public async Task<ProductDto> CreateProduct(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                BasePrice = createProductDto.BasePrice,
                IsActive = createProductDto.IsActive
            };

            foreach (var variantDto in createProductDto.Variants)
            {
                product.Variants.Add(new ProductVariant
                {
                    Color = variantDto.Color,
                    Size = variantDto.Size,
                    SKU = variantDto.SKU,
                    Stock = variantDto.Stock,
                    ImageUrl = variantDto.ImageUrl
                });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                IsActive = product.IsActive,
                Variants = product.Variants.Select(pv => new ProductVariantDto
                {
                    Id = pv.Id,
                    ProductId = pv.ProductId,
                    Color = pv.Color,
                    Size = pv.Size,
                    SKU = pv.SKU,
                    Stock = pv.Stock,
                    ImageUrl = pv.ImageUrl
                }).ToList()
            };
        }

        public async Task<ProductDto?> UpdateProduct(Guid id, CreateProductDto updateProductDto)
        {
            var product = await _context.Products
                                        .Include(p => p.Variants)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.BasePrice = updateProductDto.BasePrice;
            product.IsActive = updateProductDto.IsActive;

            _context.ProductVariants.RemoveRange(product.Variants);
            foreach (var variantDto in updateProductDto.Variants)
            {
                product.Variants.Add(new ProductVariant
                {
                    ProductId = product.Id,
                    Color = variantDto.Color,
                    Size = variantDto.Size,
                    SKU = variantDto.SKU,
                    Stock = variantDto.Stock,
                    ImageUrl = variantDto.ImageUrl
                });
            }

            await _context.SaveChangesAsync();

            return await GetProductById(product.Id);
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductDto>> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllProducts();
            }

            var lowerSearchTerm = searchTerm.ToLower();

            var products = await _context.Products
                                         .Include(p => p.Variants)
                                         .Where(p => p.IsActive &&
                                                     (p.Name.ToLower().Contains(lowerSearchTerm) ||
                                                      (p.Description != null && p.Description.ToLower().Contains(lowerSearchTerm)) ||
                                                      p.Variants.Any(pv => pv.Color.ToLower().Contains(lowerSearchTerm) ||
                                                                           pv.Size.ToLower().Contains(lowerSearchTerm) ||
                                                                           pv.SKU.ToLower().Contains(lowerSearchTerm))))
                                         .ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                IsActive = p.IsActive,
                Variants = p.Variants.Select(pv => new ProductVariantDto
                {
                    Id = pv.Id,
                    ProductId = pv.ProductId,
                    Color = pv.Color,
                    Size = pv.Size,
                    SKU = pv.SKU,
                    Stock = pv.Stock,
                    ImageUrl = pv.ImageUrl
                }).ToList()
            });
        }
    }
}
