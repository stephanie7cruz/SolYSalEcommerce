using Microsoft.EntityFrameworkCore;
using SolYSalEcommerce.Data;
using SolYSalEcommerce.DTOs.Products;
using SolYSalEcommerce.Models;
using SolYSalEcommerce.Services.Interfaces;
using System; // Para Guid
using System.Collections.Generic;
using System.Linq; // Necesario para .ToHashSet()

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
                    ImageUrl = pv.ImageUrl,
                    BasePrice = pv.BasePrice // Asegúrate de mapear BasePrice aquí también
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
                    ImageUrl = pv.ImageUrl,
                    BasePrice = pv.BasePrice // Asegúrate de mapear BasePrice aquí también
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
                IsActive = createProductDto.IsActive,
                Variants = new List<ProductVariant>() // Asegura que la colección está inicializada
            };

            foreach (var variantDto in createProductDto.Variants)
            {
                // Lógica para asignar el BasePrice de la variante
                // Si variantDto.BasePrice es nulo, usa el BasePrice del producto padre
                var variantBasePrice = variantDto.BasePrice ?? product.BasePrice;

                // Validar que el precio de la variante sea mayor que cero
                if (variantBasePrice <= 0)
                {
                    // Puedes lanzar una excepción, loggear una advertencia, o asignar un valor predeterminado si es un error.
                    throw new ArgumentException($"El precio base de la variante con SKU '{variantDto.SKU}' debe ser mayor a cero.");
                }

                product.Variants.Add(new ProductVariant
                {
                    Color = variantDto.Color,
                    Size = variantDto.Size,
                    SKU = variantDto.SKU,
                    Stock = variantDto.Stock,
                    ImageUrl = variantDto.ImageUrl,
                    BasePrice = variantBasePrice // Asigna el precio calculado
                });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Retorna el DTO del producto creado (recargado para obtener IDs de variantes)
            // Es mejor recargar desde la DB para asegurar que los IDs de las variantes generados por EF Core estén disponibles
            return await GetProductById(product.Id) ?? throw new InvalidOperationException("Product not found after creation.");
        }

        public async Task<ProductDto?> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _context.Products
                                                .Include(p => p.Variants) // ¡MUY IMPORTANTE cargar las variantes existentes!
                                                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return null; // Producto no encontrado
            }

            // 1. Actualizar las propiedades del producto principal
            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.BasePrice = updateProductDto.BasePrice;
            existingProduct.IsActive = updateProductDto.IsActive;

            // --- Lógica para manejar las variantes ---

            // Crear un conjunto de IDs de las variantes que vienen en el DTO para identificar las que deben existir
            var incomingVariantIds = updateProductDto.Variants
                                                     .Where(vDto => vDto.Id.HasValue && vDto.Id != Guid.Empty)
                                                     .Select(vDto => vDto.Id.Value)
                                                     .ToHashSet();

            // Identificar variantes a eliminar (están en DB pero no en el DTO de actualización)
            var variantsToRemove = existingProduct.Variants
                                                 .Where(existingVariant => !incomingVariantIds.Contains(existingVariant.Id))
                                                 .ToList();

            foreach (var variantToRemove in variantsToRemove)
            {
                _context.ProductVariants.Remove(variantToRemove); // Marcar para eliminación
            }

            // Identificar y procesar variantes a actualizar o añadir
            foreach (var variantDto in updateProductDto.Variants)
            {
                // Lógica para asignar el BasePrice de la variante
                // Si variantDto.BasePrice es nulo, usa el BasePrice del producto padre
                var variantBasePrice = variantDto.BasePrice ?? existingProduct.BasePrice;

                // Validar que el precio de la variante sea mayor que cero
                if (variantBasePrice <= 0)
                {
                    throw new ArgumentException($"El precio base de la variante con SKU '{variantDto.SKU}' debe ser mayor a cero.");
                }

                if (variantDto.Id.HasValue && variantDto.Id != Guid.Empty)
                {
                    // Es una variante existente: intentar encontrarla y actualizarla
                    var existingVariant = existingProduct.Variants
                                                        .FirstOrDefault(v => v.Id == variantDto.Id.Value);

                    if (existingVariant != null)
                    {
                        // Actualizar propiedades de la variante existente
                        existingVariant.Color = variantDto.Color;
                        existingVariant.Size = variantDto.Size;
                        existingVariant.SKU = variantDto.SKU;
                        existingVariant.Stock = variantDto.Stock;
                        existingVariant.ImageUrl = variantDto.ImageUrl;
                        existingVariant.BasePrice = variantBasePrice; // Asigna el precio calculado
                        // EF Core detectará los cambios automáticamente.
                    }
                    else
                    {
                        // Advertencia: El cliente envió un ID de variante existente que no corresponde a este producto
                        // o es un ID inválido. Podrías lanzar una excepción o loggear.
                        // Por ahora, para evitar problemas, lo trataremos como una nueva variante si el ID no se encuentra en la colección cargada.
                        // Sin embargo, un manejo más robusto podría ser devolver un error 400.
                        Console.WriteLine($"Advertencia: Variane con ID {variantDto.Id} no encontrada para actualizar en el producto {id}. Se tratará como nueva.");
                        existingProduct.Variants.Add(new ProductVariant
                        {
                            ProductId = existingProduct.Id,
                            Color = variantDto.Color,
                            Size = variantDto.Size,
                            SKU = variantDto.SKU,
                            Stock = variantDto.Stock,
                            ImageUrl = variantDto.ImageUrl,
                            BasePrice = variantBasePrice // Asigna el precio calculado
                        });
                    }
                }
                else
                {
                    // Es una nueva variante: añadirla (sin ID inicial)
                    var newVariant = new ProductVariant
                    {
                        ProductId = existingProduct.Id, // Asegura la relación con el producto padre
                        Color = variantDto.Color,
                        Size = variantDto.Size,
                        SKU = variantDto.SKU,
                        Stock = variantDto.Stock,
                        ImageUrl = variantDto.ImageUrl,
                        BasePrice = variantBasePrice // Asigna el precio calculado
                    };
                    existingProduct.Variants.Add(newVariant); // Añade la nueva variante a la colección
                }
            }

            // 3. Guardar los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error de concurrencia al actualizar producto {id}: {ex.Message}");
                // Loguea los detalles del error para depuración
                return null; // O relanzar la excepción
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar producto {id}: {ex.Message}");
                // Loguea otros tipos de excepciones
                return null; // O relanzar la excepción
            }

            // 4. Devolver el producto actualizado (recargado para asegurar los datos más recientes)
            // Esto es crucial para que los IDs de las nuevas variantes se reflejen en el DTO de respuesta.
            return await GetProductById(existingProduct.Id);
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
                    ImageUrl = pv.ImageUrl,
                    BasePrice = pv.BasePrice // Asegúrate de mapear BasePrice aquí también
                }).ToList()
            });
        }
    }
}