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
    private readonly IValidator<CreateCategoryRequest> _createCategoryValidator;
    private readonly IValidator<UpdateCategoryRequest> _updateCategoryValidator;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork<Category> unitOfWork,
        IValidator<CreateCategoryRequest> createCategoryValidator,
        IValidator<UpdateCategoryRequest> updateCategoryValidator,
        ILogger<CategoryService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _createCategoryValidator = createCategoryValidator;
        _updateCategoryValidator = updateCategoryValidator;
        _logger = logger;
        _mapper = mapper;
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

    public async Task<Category?> CreateCategory(CreateCategoryRequest createCategory)
    {
        var result = _createCategoryValidator.Validate(createCategory);
        if (!result.IsValid)
        {
            //_logger.LogError("CreateCategoryRequest object errors:\nerrors", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        Category category = _mapper.Map<Category>(createCategory);

        await _unitOfWork.GenericRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateCategory(UpdateCategoryRequest updateCategory)
    {
        var result = _updateCategoryValidator.Validate(updateCategory);
        if (!result.IsValid)
        {
            //_logger.LogError("UpdateCategoryRequest object errors:\n", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        Category category = _mapper.Map<Category>(updateCategory);

        _unitOfWork.GenericRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();
        return category;
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