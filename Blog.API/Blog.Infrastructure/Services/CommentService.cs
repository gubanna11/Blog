using Blog.Core.Contracts.Controllers.Comments;
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

    public async Task<IEnumerable<CommentResponse>> GetComments(
        CancellationToken cancellationToken)
    {
        var comments = await _unitOfWork.GenericRepository.Set
            .Include(c => c.User)
            .ToListAsync(cancellationToken);

        foreach (var comment in comments)
        {
            if (comment.User is not null)
            {
                comment.User.Comments = null;
            }
        }

        return _mapper.Map<IEnumerable<CommentResponse>>(comments);
    }

    public async Task<CommentResponse?> GetCommentById(Guid id, CancellationToken cancellationToken)
    {
        var comment = await _unitOfWork.GenericRepository.Set
            .Where(c => c.CommentId == id)
            .Include(c => c.User)
            .FirstOrDefaultAsync(cancellationToken);

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

    public async Task<CommentResponse?> CreateComment(CreateCommentRequest createComment,
        CancellationToken cancellationToken)
    {
        var comment = _mapper.Map<Comment>(createComment);

        comment.PublishDate = DateTime.Now;

        await _unitOfWork.GenericRepository.AddAsync(comment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CommentResponse>(comment);
    }

    public async Task<CommentResponse?> UpdateComment(UpdateCommentRequest updateComment,
        CancellationToken cancellationToken)
    {
        var comment = _mapper.Map<Comment>(updateComment);

        _unitOfWork.GenericRepository.Update(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CommentResponse>(comment);
    }

    public async Task<CommentResponse?> DeleteComment(Guid id, CancellationToken cancellationToken)
    {
        var comment = await _unitOfWork.GenericRepository.RemoveAsync(id, cancellationToken);

        if (comment is not null)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CommentResponse>(comment);
        }

        return null;
    }
}