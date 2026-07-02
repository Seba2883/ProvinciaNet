using ApiInventory.Application.DTOs.Product;
using ApiInventory.Domain.Entities;
using ApiInventory.Infrastructure.Services;
using ApiInventory.Tests.Helpers;

namespace ApiInventory.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateProduct_WhenCategoryExists()
    {
        // Arrange

        var context = DbContextFactory.Create();

        var category = new Category
        {
            Name = "Hardware",
            Description = "Hardware"
        };

        context.Categories.Add(category);

        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var dto = new CreateProductDto
        {
            Name = "Mouse Logitech",
            Description = "Wireless",
            Price = 100,
            Stock = 5,
            CategoryId = category.Id
        };

        // Act

        var result = await service.CreateAsync(dto);

        // Assert

        Assert.NotNull(result);

        Assert.Equal(dto.Name, result.Name);

        Assert.Equal(dto.Price, result.Price);

        Assert.Equal(dto.Stock, result.Stock);
                
        Assert.Equal(category.Id, result.CategoryId);
        
        Assert.Equal(category.Name, result.CategoryName);
    }
}
