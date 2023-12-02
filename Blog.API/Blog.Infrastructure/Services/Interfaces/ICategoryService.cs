using Blog.Core.Contracts.Controllers.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetCategories();
    Task<CategoryResponse?> GetCategoryById(Guid id);
    Task<CategoryResponse?> CreateCategory(CreateCategoryRequest createCategory);
    Task<CategoryResponse?> UpdateCategory(UpdateCategoryRequest updateCategory);
    Task<CategoryResponse?> DeleteCategory(Guid id);
}