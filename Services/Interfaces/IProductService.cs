using SolYSalEcommerce.DTOs.Products;
using SolYSalEcommerce.Models; 

namespace SolYSalEcommerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto?> GetProductById(Guid id);
        Task<ProductDto> CreateProduct(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProduct(Guid id, CreateProductDto updateProductDto);
        Task<bool> DeleteProduct(Guid id);
        Task<IEnumerable<ProductDto>> SearchProducts(string searchTerm);
    }
}