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
            .Include(c => c.Post)
            .ToListAsync();

        NullNesting(comments);

        return _mapper.Map<IEnumerable<CommentResponse>>(comments);
    }

    public async Task<PagedResponse<CommentResponse>> GetPagedComments(GetPagedCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var commentsQuery = _unitOfWork.GenericRepository.Set;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            commentsQuery = commentsQuery.Where(c => c.Content.Contains(request.SearchTerm));
        }

        Expression<Func<Comment, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "content" => comment => comment.Content,
            "publishDate" => comment => comment.PublishDate,
            _ => category => category.Content,
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            commentsQuery = commentsQuery.OrderByDescending(keySelector);
        }
        else
        {
            commentsQuery = commentsQuery.OrderBy(keySelector);
        }

        if (request.IsIncludePost)
        {
            commentsQuery = commentsQuery
                .Include(c => c.Post);
        }

        if (request.IsIncludeUser)
        {
            commentsQuery = commentsQuery
                .Include(c => c.User);
        }

        var categories = await PagedResponse<CommentResponse>.CreateAsync(commentsQuery, request.Page,
            request.PageSize, MapCommentsToCommentResponses, NullNesting, cancellationToken);

        return categories;
    }

    public async Task<CursorPagedResponse<CommentResponse>> GetCursorPagedComments(
        GetCursorPagedCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var commentsQuery = _unitOfWork.GenericRepository.Set;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            commentsQuery = commentsQuery.Where(c => c.Content.Contains(request.SearchTerm));
        }

        Expression<Func<Comment, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "content" => comment => comment.Content,
            "publishDate" => comment => comment.PublishDate,
            _ => category => category.Content,
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            commentsQuery = commentsQuery.OrderByDescending(keySelector);
        }
        else
        {
            commentsQuery = commentsQuery.OrderBy(keySelector);
        }

        if (request.IsIncludePost)
        {
            commentsQuery = commentsQuery
                .Include(c => c.Post);
        }

        if (request.IsIncludeUser)
        {
            commentsQuery = commentsQuery
                .Include(c => c.User);
        }

        var categories = await CursorPagedResponse<CommentResponse>.CreateAsync(commentsQuery,
            request.PageSize, c => c.CommentId > request.Cursor, item => item?.CommentId,
            MapCommentsToCommentResponses, NullNesting, cancellationToken);

        return categories;
    }

    public async Task<CommentResponse?> GetCommentById(Guid id)
    {
        Comment? comment = await _unitOfWork.GenericRepository.Set
            .Where(c => c.CommentId == id)
            .Include(c => c.User)
            .FirstOrDefaultAsync();

        if (comment is not null)
        {
            if (comment.User is not null)
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

    private IEnumerable<CommentResponse> MapCommentsToCommentResponses(IEnumerable<Comment> comments)
    {
        return _mapper.Map<IEnumerable<CommentResponse>>(comments);
    }

    private static List<Comment> NullNesting(List<Comment> comments)
    {
        foreach (var comment in comments)
        {
            if (comment.User is not null)
            {
                comment.User.Comments = null;
                comment.User.Posts = null;
            }

            if (comment.Post is not null)
            {
                comment.Post.User = null;
                comment.Post.Category = null;
            }
        }

        return comments;
    }
}