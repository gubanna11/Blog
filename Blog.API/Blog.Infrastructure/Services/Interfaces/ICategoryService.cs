using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICategoryService
{
    IEnumerable<Category> GetCategories();
    Task<Category?> GetCategoryById(Guid id);
    Task<Category> CreateCategory(Category createCategory);
    Task<Category?> UpdateCategory(Category updateCategory);
    Task<Category?> DeleteCategory(Guid id);
}