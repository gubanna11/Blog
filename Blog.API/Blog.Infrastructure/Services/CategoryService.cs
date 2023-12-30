using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Entities;
using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

    public async Task<IEnumerable<CategoryResponse>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.GenericRepository.Set
            .Include(c => c.Posts)
            .ToListAsync(cancellationToken);

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
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<CategoryResponse?> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.GenericRepository.Set
            .Where(c => c.CategoryId == id)
            .Include(c => c.Posts)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is not null)
        {
            if (category.Posts is not null)
            {
                foreach (var post in category.Posts)
                {
                    post.Category = null;
                }
            }
            return _mapper.Map<CategoryResponse>(category);
        }

        return null;
    }

    public async Task<CategoryResponse?> CreateCategory(CreateCategoryRequest createCategory, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(createCategory);

        await _unitOfWork.GenericRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse?> UpdateCategory(UpdateCategoryRequest updateCategory, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(updateCategory);

        _unitOfWork.GenericRepository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse?> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.GenericRepository.RemoveAsync(id, cancellationToken);

        if (category is not null)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<CategoryResponse>(category);
        }

        return null;
    }
}