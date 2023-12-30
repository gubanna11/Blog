using Blog.Core.Contracts.Controllers.Categories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetCategories(CancellationToken cancellationToken);
    Task<CategoryResponse?> GetCategoryById(Guid id, CancellationToken cancellationToken);
    Task<CategoryResponse?> CreateCategory(CreateCategoryRequest createCategory, CancellationToken cancellationToken);
    Task<CategoryResponse?> UpdateCategory(UpdateCategoryRequest updateCategory, CancellationToken cancellationToken);
    Task<CategoryResponse?> DeleteCategory(Guid id, CancellationToken cancellationToken);
}