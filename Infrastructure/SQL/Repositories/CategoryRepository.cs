using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Repositories;
using Hello.NET.Exceptions;
using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Hello.NET.Infrastructure.SQL.Repositories;

public sealed class CategoryRepository(ApplicationDbContext context)
    : ICategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<CategoryEntity>> GetCategoriesAsync(
        PagingDto paging
    ) =>
        await _context
            .Categories.AsNoTracking()
            .Skip((paging.PageIndex - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync();

    public async Task<CategoryEntity?> GetCategoryAsync(long id) =>
        await _context
            .Categories.AsNoTracking()
            .FirstOrDefaultAsync(category => category.ID == id);

    public async Task<bool> CheckCategoryAsync(long id) =>
        await _context.Categories.AnyAsync(category => category.ID == id);

    public async Task<long> CreateCategoryAsync(CategoryEntity category)
    {
        await _context.Categories.AddAsync(category);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is NpgsqlException _ex
                && _ex.SqlState == "23505"
            )
        {
            throw new DataConflictException("Category name already exists", ex);
        }
        return category.ID;
    }

    public async Task<List<long>> CreateCategoriesAsync(
        List<CategoryEntity> categories
    )
    {
        await _context.Categories.AddRangeAsync(categories);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is NpgsqlException _ex
                && _ex.SqlState == "23505"
            )
        {
            throw new DataConflictException("Category name already exists", ex);
        }
        return [.. categories.Select(category => category.ID)];
    }

    public async Task<int> UpdateCategoryAsync(
        long id,
        CategoryEntity category
    ) =>
        await _context
            .Categories.Where(_category => _category.ID == id)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(_category => _category.Name, category.Name)
            );

    public async Task<int> DeleteCategoryAsync(long id) =>
        await _context
            .Categories.Where(category => category.ID == id)
            .ExecuteDeleteAsync();
}
