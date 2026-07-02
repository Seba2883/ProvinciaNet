using ApiInventory.Application.Constants;
using ApiInventory.Application.DTOs.Product;
using ApiInventory.Application.Exceptions;
using ApiInventory.Application.Interfaces.Services;
using ApiInventory.Application.Mappings;
using ApiInventory.Domain.Entities;
using ApiInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiInventory.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _context;

    public ProductService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(ProductMappings.ToDto)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductDto> GetByIdAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Select(ProductMappings.ToDto)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return product
            ?? throw new NotFoundException(ErrorMessages.ProductNotFound(id));
    }

    public async Task<ProductDto> CreateAsync(
        CreateProductDto dto,
        CancellationToken cancellationToken = default)
    {
        await ValidateCategoryExistsAsync(dto.CategoryId, cancellationToken);

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == product.Id)
            .Select(ProductMappings.ToDto)
            .FirstAsync(cancellationToken);
    }

    public async Task UpdateAsync(
      int id,
      UpdateProductDto dto,
      CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);

        if (product.CategoryId != dto.CategoryId)
        {
            await ValidateCategoryExistsAsync(dto.CategoryId, cancellationToken);
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);

        _context.Products.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> GetProductOrThrowAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new NotFoundException(ErrorMessages.ProductNotFound(id));
    }

    private async Task ValidateCategoryExistsAsync(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Categories
            .AnyAsync(c => c.Id == categoryId, cancellationToken);

        if (!exists)
        {
            throw new NotFoundException(ErrorMessages.CategoryNotFoundForProduct);
        }
    }
}