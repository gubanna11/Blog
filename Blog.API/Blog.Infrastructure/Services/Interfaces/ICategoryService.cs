using Blog.Core.Contracts.Controllers.Categories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.MediatR.Queries.Categories;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetCategories(CancellationToken cancellationToken);
    Task<PagedResponse<CategoryResponse>> GetPagedCategories(GetPagedCategoriesQuery request, CancellationToken cancellationToken);
    Task<CursorPagedResponse<CategoryResponse>> GetCursorPagedCategories(GetCursorPagedCategoriesQuery request, CancellationToken cancellationToken);
    Task<CategoryResponse?> GetCategoryById(Guid id, CancellationToken cancellationToken);
    Task<CategoryResponse?> CreateCategory(CreateCategoryRequest createCategory, CancellationToken cancellationToken);
    Task<CategoryResponse?> UpdateCategory(UpdateCategoryRequest updateCategory, CancellationToken cancellationToken);
    Task<CategoryResponse?> DeleteCategory(Guid id, CancellationToken cancellationToken);
}