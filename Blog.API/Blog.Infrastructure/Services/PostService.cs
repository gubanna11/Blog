using Blog.Core.Contracts.Controllers.Posts;
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

public sealed class PostService : IPostService
{
    private readonly IUnitOfWork<Post> _unitOfWork;
    private readonly IMapper _mapper;

    public PostService(IUnitOfWork<Post> unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostResponse>> GetPosts(CancellationToken cancellationToken)
    {
        var posts = await _unitOfWork.GenericRepository.Set
            .Include(p => p.Category)
            .Include(p => p.User)
            .ToListAsync(cancellationToken);

        foreach (var post in posts)
        {
            if (post.User is not null)
            {
                post.User.Posts = null;
            }

            if (post.Category is not null)
            {
                post.Category.Posts = null;
            }
        }

        return _mapper.Map<IEnumerable<PostResponse>>(posts);
    }

    public async Task<PostResponse?> GetPostById(Guid id, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.GenericRepository.Set
            .Where(p => p.PostId == id)
            .Include(p => p.Category)
            .Include(p => p.User)
            .FirstOrDefaultAsync(cancellationToken);

        if (post is not null)
        {
            if (post.User is not null)
            {
                post.User.Posts = null;
            }

            if (post.Category is not null)
            {
                post.Category.Posts = null;
            }

            return _mapper.Map<PostResponse>(post);
        }

        return null;
    }

    public async Task<PostResponse?> CreatePost(CreatePostRequest createPost, CancellationToken cancellationToken)
    {
        var post = _mapper.Map<Post>(createPost);

        post.PublishDate = DateTime.Now;

        await _unitOfWork.GenericRepository.AddAsync(post, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PostResponse>(post);
    }

    public async Task<PostResponse?> UpdatePost(UpdatePostRequest updatePost, CancellationToken cancellationToken)
    {
        var post = _mapper.Map<Post>(updatePost);

        _unitOfWork.GenericRepository.Update(post);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PostResponse>(post);
    }

    public async Task<PostResponse?> DeletePost(Guid id, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.GenericRepository.RemoveAsync(id, cancellationToken);

        if (post is not null)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<PostResponse>(post);
        }

        return null;
    }
}