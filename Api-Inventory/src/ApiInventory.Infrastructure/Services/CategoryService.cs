using ApiInventory.Application.Constants;
using ApiInventory.Application.DTOs.Category;
using ApiInventory.Application.Exceptions;
using ApiInventory.Application.Interfaces.Services;
using ApiInventory.Application.Mappings;
using ApiInventory.Domain.Entities;
using ApiInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiInventory.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly InventoryDbContext _context;

    public CategoryService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(CategoryMappings.ToDto)
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .Select(CategoryMappings.ToDto)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return category
            ?? throw new NotFoundException(ErrorMessages.CategoryNotFound(id));
    }

    public async Task<CategoryDto> CreateAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        await ValidateCategoryNameAsync(dto.Name, null, cancellationToken);

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.Id == category.Id)
            .Select(CategoryMappings.ToDto)
            .FirstAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = await GetCategoryOrThrowAsync(id, cancellationToken);

        if (!string.Equals(category.Name, dto.Name, StringComparison.OrdinalIgnoreCase))
        {
            await ValidateCategoryNameAsync(dto.Name, id, cancellationToken);
        }

        category.Name = dto.Name;
        category.Description = dto.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await GetCategoryOrThrowAsync(id, cancellationToken);

        await ValidateCategoryHasNoProductsAsync(id, cancellationToken);

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<Category> GetCategoryOrThrowAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new NotFoundException(ErrorMessages.CategoryNotFound(id));
    }

    private async Task ValidateCategoryHasNoProductsAsync(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var hasProducts = await _context.Products
            .AnyAsync(p => p.CategoryId == categoryId, cancellationToken);

        if (hasProducts)
        {
            throw new BusinessException(ErrorMessages.CategoryHasActiveProducts);
        }
    }

    private async Task ValidateCategoryNameAsync(
        string name,
        int? categoryId,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Categories.AnyAsync(c =>
            c.Name == name &&
            (!categoryId.HasValue || c.Id != categoryId.Value),
            cancellationToken);

        if (exists)
        {
            throw new BusinessException(ErrorMessages.CategoryAlreadyExists(name));
        }
    }
}