using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Entities;
using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Services.Interfaces;
using FluentValidation;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly IUnitOfWork<Category> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork<Category> unitOfWork,
        ILogger<CategoryService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategories()
    {
        var categories = await _unitOfWork.GenericRepository.Set
            .Include(c => c.Posts)
            .ToListAsync();

        foreach (var category in categories)
        {
            if (category.Posts is not null)
            {
                foreach (Post post in category.Posts)
                {
                    post.Category = null;
                }
            }
        }
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
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

        _logger.LogError("Category object with id {GetByIdCategoryId} doesn't exist", id);
        return null;
    }

    public async Task<CategoryResponse?> CreateCategory(CreateCategoryRequest createCategory)
    {
        Category category = _mapper.Map<Category>(createCategory);

        await _unitOfWork.GenericRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse?> UpdateCategory(UpdateCategoryRequest updateCategory)
    {
        Category category = _mapper.Map<Category>(updateCategory);

        _unitOfWork.GenericRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse?> DeleteCategory(Guid id)
    {
        Category? category = _unitOfWork.GenericRepository.Remove(id);

        if (category is not null)
        {
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryResponse>(category);
        }

        _logger.LogError("Category object with id {DeleteCategoryId} doesn't exist", id);
        return null;
    }
}