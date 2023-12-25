using Blog.Core.Contracts.Controllers.Posts;
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
using Blog.Core.MediatR.Queries.Posts;

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

    public async Task<IEnumerable<PostResponse>> GetPosts()
    {
        var posts = await _unitOfWork.GenericRepository.Set
            .Include(p => p.Category)
            .Include(p => p.User)
            .ToListAsync();

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

    public async Task<PagedResponse<PostResponse>> GetPagedPosts(GetPagedPostsQuery request,
        CancellationToken cancellationToken)
    {
        var postsQuery = _unitOfWork.GenericRepository.Set;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            postsQuery = postsQuery.Where(p => p.Title.Contains(request.SearchTerm));
        }

        Expression<Func<Post, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "title" => post => post.Title,
            "content" => post => post.Content,
            "publishDate" => post => post.PublishDate,
            _ => post => post.Title,
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            postsQuery = postsQuery.OrderByDescending(keySelector);
        }
        else
        {
            postsQuery = postsQuery.OrderBy(keySelector);
        }

        if (request.IsIncludeCategory)
        {
            postsQuery = postsQuery
                .Include(p => p.Category);
        }

        if (request.IsIncludeUser)
        {
            postsQuery = postsQuery
                .Include(p => p.User);
        }

        var posts = await PagedResponse<PostResponse>.CreateAsync(postsQuery, request.Page,
            request.PageSize, MapPostsToPostResponses, NullNesting, cancellationToken);

        return posts;
    }

    public async Task<CursorPagedResponse<PostResponse>> GetCursorPagedPosts(
        GetCursorPagedPostsQuery request,
        CancellationToken cancellationToken)
    {
        var postsQuery = _unitOfWork.GenericRepository.Set;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            postsQuery = postsQuery.Where(p => p.Title.Contains(request.SearchTerm));
        }

        Expression<Func<Post, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "title" => post => post.Title,
            "content" => post => post.Content,
            "publishDate" => post => post.PublishDate,
            _ => post => post.Title,
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            postsQuery = postsQuery.OrderByDescending(keySelector);
        }
        else
        {
            postsQuery = postsQuery.OrderBy(keySelector);
        }

        if (request.IsIncludeCategory)
        {
            postsQuery = postsQuery
                .Include(p => p.Category);
        }

        if (request.IsIncludeUser)
        {
            postsQuery = postsQuery
                .Include(p => p.User);
        }

        var posts = await CursorPagedResponse<PostResponse>.CreateAsync(postsQuery,
            request.PageSize, c => c.PostId > request.Cursor, item => item?.PostId,
            MapPostsToPostResponses, NullNesting, cancellationToken);

        return posts;
    }

    public async Task<PostResponse?> GetPostById(Guid id)
    {
        Post? post = await _unitOfWork.GenericRepository.Set
            .Where(p => p.PostId == id)
            .Include(p => p.Category)
            .Include(p => p.User)
            .FirstOrDefaultAsync();

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

    public async Task<PostResponse?> CreatePost(CreatePostRequest createPost)
    {
        Post post = _mapper.Map<Post>(createPost);

        post.PublishDate = DateTime.Now;

        await _unitOfWork.GenericRepository.AddAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PostResponse>(post);
    }

    public async Task<PostResponse?> UpdatePost(UpdatePostRequest updatePost)
    {
        Post post = _mapper.Map<Post>(updatePost);

        _unitOfWork.GenericRepository.Update(post);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PostResponse>(post);
    }

    public async Task<PostResponse?> DeletePost(Guid id)
    {
        Post? post = _unitOfWork.GenericRepository.Remove(id);

        if (post is not null)
        {
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PostResponse>(post);
        }

        return null;
    }

    private IEnumerable<PostResponse> MapPostsToPostResponses(IEnumerable<Post> posts)
    {
        return _mapper.Map<IEnumerable<PostResponse>>(posts);
    }

    private static List<Post> NullNesting(List<Post> posts)
    {
        foreach (var post in posts)
        {
            if (post.User is not null)
            {
                post.User.Comments = null;
                post.User.Posts = null;
            }

            if (post.Category is not null)
            {
                post.Category.Posts = null;
            }
        }

        return posts;
    }
}