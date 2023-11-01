using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategories();
    Task<Category?> GetCategoryById(Guid id);
    Task<Category?> CreateCategory(CreateCategoryRequest createCategory);
    Task<Category?> UpdateCategory(UpdateCategoryRequest updateCategory);
    Task<Category?> DeleteCategory(Guid id);
}