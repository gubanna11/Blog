using Blog.Core.Contracts.Controllers.Comments;
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

public sealed class CommentService : ICommentService
{
    private readonly IUnitOfWork<Comment> _unitOfWork;
    private readonly ILogger<CommentService> _logger;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork<Comment> unitOfWork,
        ILogger<CommentService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
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

        _logger.LogError("Comment object with id {id} doesn't exits", id);
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

        _logger.LogError("Comment object with id {id} doesn't exist", id);
        return null;
    }
}