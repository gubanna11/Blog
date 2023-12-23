using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Entities;
using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.MediatR.Queries.Categories;

namespace Blog.Infrastructure.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly IUnitOfWork<Category> _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork<Category> unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategories()
    {
        var categories = await _unitOfWork.GenericRepository.Set
            .Include(c => c.Posts)
            .ToListAsync();

        NullCategories(categories);

        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<PagedResponse<CategoryResponse>> GetPagedCategories(GetPagedCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categoriesQuery = _unitOfWork.GenericRepository.Set;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            categoriesQuery = categoriesQuery.Where(c => c.Name.Contains(request.SearchTerm));
        }

        Expression<Func<Category, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "name" => category => category.Name,
            _ => category => category.CategoryId,
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            categoriesQuery = categoriesQuery.OrderByDescending(keySelector);
        }
        else
        {
            categoriesQuery = categoriesQuery.OrderBy(keySelector);
        }

        categoriesQuery = categoriesQuery
            .Include(c => c.Posts);

        var categories = await PagedResponse<CategoryResponse>.CreateAsync(categoriesQuery, request.Page,
            request.PageSize, MapFunction, NullCategories, cancellationToken);

        return categories;

        IEnumerable<CategoryResponse> MapFunction(IEnumerable<Category> mappedCategories) =>
            _mapper.Map<IEnumerable<CategoryResponse>>(mappedCategories);
    }

    public async Task<CursorPagedResponse<CategoryResponse>> GetCursorPagedCategories(
        GetCursorPagedCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categoriesQuery = _unitOfWork.GenericRepository.Set
            .Include(c => c.Posts)
            .Select(c => new CategoryResponse
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Posts = c.Posts
            });

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            categoriesQuery = categoriesQuery.Where(c => c.Name.Contains(request.SearchTerm));
        }

        Expression<Func<CategoryResponse, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "name" => category => category.Name,
            _ => category => category.CategoryId,
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            categoriesQuery = categoriesQuery.OrderByDescending(keySelector);
        }
        else
        {
            categoriesQuery = categoriesQuery.OrderBy(keySelector);
        }

        var categories = await CursorPagedResponse<CategoryResponse>.CreateAsync(categoriesQuery, request.Cursor,
            request.PageSize, c => c.CategoryId > request.Cursor, item => item?.CategoryId, cancellationToken);

        foreach (var category in categories.Items)
        {
            if (category.Posts is not null)
            {
                foreach (var post in category.Posts)
                {
                    post.Category = null;
                }
            }
        }

        return categories;
    }


    public async Task<CategoryResponse?> GetCategoryById(Guid id)
    {
        Category? category = await _unitOfWork.GenericRepository.Set
            .Where(c => c.CategoryId == id)
            .Include(c => c.Posts)
            .FirstOrDefaultAsync();

        if (category is not null)
        {
            if (category.Posts is not null)
            {
                foreach (Post post in category.Posts)
                {
                    post.Category = null;
                }
            }

            return _mapper.Map<CategoryResponse>(category);
        }

        return null;
    }

    public async Task<CategoryResponse?> CreateCategory(CreateCategoryRequest createCategory)
    {
        var category = _mapper.Map<Category>(createCategory);

        await _unitOfWork.GenericRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse?> UpdateCategory(UpdateCategoryRequest updateCategory)
    {
        var category = _mapper.Map<Category>(updateCategory);

        _unitOfWork.GenericRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse?> DeleteCategory(Guid id)
    {
        var category = _unitOfWork.GenericRepository.Remove(id);

        if (category is not null)
        {
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryResponse>(category);
        }

        return null;
    }

    private static List<Category> NullCategories(List<Category> categories)
    {
        foreach (var category in categories)
        {
            if (category.Posts is not null)
            {
                foreach (var post in category.Posts)
                {
                    post.Category = null;
                }
            }
        }

        return categories;
    }
}