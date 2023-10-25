using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Infrastructure.Services.Interfaces;

namespace Blog.Infrastructure.Services;

//TODO:Implement methods
public sealed class CategoryService : ICategoryService
{
    public IEnumerable<Category> GetCategories()
    {
        throw new NotImplementedException();
    }

    public Task<Category?> GetCategoryById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Category> CreateCategory(Category createCategory)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> UpdateCategory(Category updateCategory)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> DeleteCategory(Guid id)
    {
        throw new NotImplementedException();
    }
}