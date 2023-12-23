using Blog.Core.Contracts.Controllers.Comments;
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
using Blog.Core.MediatR.Queries.Comments;

namespace Blog.Infrastructure.Services;

public sealed class CommentService : ICommentService
{
    private readonly IUnitOfWork<Comment> _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork<Comment> unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentResponse>> GetComments()
    {
        var comments = await _unitOfWork.GenericRepository.Set
            .Include(c => c.User)
            .ToListAsync();

        foreach (var comment in comments)
        {
            if (comment.User is not null)
            {
                comment.User.Comments = null;
            }
        }

        return _mapper.Map<IEnumerable<CommentResponse>>(comments);
    }

    public async Task<PagedResponse<Comment>> GetPagedComments(GetPagedCommentsQuery request, CancellationToken cancellationToken)
    {
        var commentsQuery = _unitOfWork.GenericRepository.Set;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            commentsQuery = commentsQuery.Where(c => c.Content.Contains(request.SearchTerm));
        }

        Expression<Func<Comment, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "content" => comment => comment.Content,
            _ => comment => comment.CommentId,
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            commentsQuery = commentsQuery.OrderByDescending(keySelector);
        }
        else
        {
            commentsQuery = commentsQuery.OrderBy(keySelector);
        }

        var commentsResponseQuery = commentsQuery
            .Include(c => c.User);

        var comments = await PagedResponse<Comment>.CreateAsync(commentsResponseQuery, request.Page, request.PageSize, cancellationToken);

        foreach (var comment in comments.Items)
        {
            if (comment.User is not null)
            {
                comment.User.Comments = null;
            }
        }

        return comments;
    }

    public async Task<CommentResponse?> GetCommentById(Guid id)
    {
        Comment? comment = await _unitOfWork.GenericRepository.Set
            .Where(c => c.CommentId == id)
            .Include(c => c.User)
            .FirstOrDefaultAsync();

        if (comment is not null)
        {
            if(comment.User is not null)
            {
                comment.User.Comments = null;
            }
            return _mapper.Map<CommentResponse>(comment);
        }

        return null;
    }

    public async Task<CommentResponse?> CreateComment(CreateCommentRequest createComment)
    {
        Comment comment = _mapper.Map<Comment>(createComment);

        comment.PublishDate = DateTime.Now;

        await _unitOfWork.GenericRepository.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CommentResponse>(comment);
    }

    public async Task<CommentResponse?> UpdateComment(UpdateCommentRequest updateComment)
    {
        Comment comment = _mapper.Map<Comment>(updateComment);

        _unitOfWork.GenericRepository.Update(comment);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CommentResponse>(comment);
    }

    public async Task<CommentResponse?> DeleteComment(Guid id)
    {
        Comment? comment = _unitOfWork.GenericRepository.Remove(id);

        if (comment is not null)
        {
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentResponse>(comment);
        }

        return null;
    }
}