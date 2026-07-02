using System.Linq.Expressions;
using ApiInventory.Application.DTOs.Category;
using ApiInventory.Domain.Entities;

namespace ApiInventory.Application.Mappings;

public static class CategoryMappings
{
    public static readonly Expression<Func<Category, CategoryDto>> ToDto =
        category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
}
