using System.Linq.Expressions;
using ApiInventory.Application.DTOs.Product;
using ApiInventory.Domain.Entities;

namespace ApiInventory.Application.Mappings;

public static class ProductMappings
{
    public static readonly Expression<Func<Product, ProductDto>> ToDto =
        product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name
        };
}