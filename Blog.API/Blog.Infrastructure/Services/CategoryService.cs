using Blog.Core.Entities;
using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly IUnitOfWork<Category> _unitOfWork;

    public CategoryService(IUnitOfWork<Category> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var categories = await _unitOfWork.GenericRepository.Set
            .Include(c => c.Posts)
            .ToListAsync();

        foreach (var category in categories)
        {
            if (category.Posts != null)
            {
                foreach (Post post in category.Posts)
                {
                    post.Category = null;
                }
            }
        }

        return categories;
    }

    public async Task<Category?> GetCategoryById(Guid id)
    {
        Category? category = await _unitOfWork.GenericRepository.Set
            .Where(c => c.CategoryId == id)
            .Include(c => c.Posts)
            .FirstOrDefaultAsync();

        return category;
    }

    public async Task<Category> CreateCategory(Category createCategory)
    {
        await _unitOfWork.GenericRepository.AddAsync(createCategory);
        await _unitOfWork.SaveChangesAsync();
        return createCategory;
    }

    public async Task<Category?> UpdateCategory(Category updateCategory)
    {
        _unitOfWork.GenericRepository.Update(updateCategory);
        await _unitOfWork.SaveChangesAsync();
        return updateCategory;
    }

    public async Task<Category?> DeleteCategory(Guid id)
    {
        Category? category = _unitOfWork.GenericRepository.Remove(id);

        if (category is not null)
        {
            await _unitOfWork.SaveChangesAsync();
            return category;
        }

        return null;
    }
}