﻿using System;
using SolYSalEcommerce.DTOs.Orders;

namespace SolYSalEcommerce.DTOs.Orders
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductVariantId { get; set; }

        public ProductVariantInOrderDto ProductVariantDetails { get; set; } = new ProductVariantInOrderDto();

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}