using ApiInventory.Application.Exceptions;
using ApiInventory.Domain.Entities;
using ApiInventory.Infrastructure.Services;
using ApiInventory.Tests.Helpers;

namespace ApiInventory.Tests.Services;

public class CategoryServiceTests
{
    [Fact]
    public async Task DeleteAsync_ShouldThrowBusinessException_WhenCategoryHasProducts()
    {
        // Arrange

        var context = DbContextFactory.Create();

        var category = new Category
        {
            Name = "Hardware"
        };

        context.Categories.Add(category);

        await context.SaveChangesAsync();

        context.Products.Add(new Product
        {
            Name = "Mouse",
            Price = 100,
            Stock = 5,
            CategoryId = category.Id
        });

        await context.SaveChangesAsync();

        var service = new CategoryService(context);

        // Act & Assert

        await Assert.ThrowsAsync<BusinessException>(() =>
            service.DeleteAsync(category.Id));
    }
}