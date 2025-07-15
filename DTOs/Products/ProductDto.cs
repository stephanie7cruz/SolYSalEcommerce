﻿namespace SolYSalEcommerce.DTOs.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    }
}