﻿using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Entities;
using Blog.Core.ResponseDtos;
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

public sealed class PostService : IPostService
{
    private readonly IUnitOfWork<Post> _unitOfWork;
    private readonly IValidator<CreatePostRequest> _createPostValidator;
    private readonly IValidator<UpdatePostRequest> _updatePostValidator;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public PostService(IUnitOfWork<Post> unitOfWork,
        IValidator<CreatePostRequest> createPostValidator,
        IValidator<UpdatePostRequest> updatePostValidator,
        ILogger<PostService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _createPostValidator = createPostValidator;
        _updatePostValidator = updatePostValidator;
        _logger = logger;
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
            if (post.User != null)
            {
                post.User.Posts = null;
            }

            if (post.Category != null)
            {
                post.Category.Posts = null;
            }
        }

        return _mapper.Map<IEnumerable<PostResponse>>(posts);
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
            if (post.User != null)
            {
                post.User.Posts = null;
            }

            if (post.Category != null)
            {
                post.Category.Posts = null;
            }

            return _mapper.Map<PostResponse>(post);
        }

        _logger.LogError("Object with id {id} doesn't exits", id);
        return null;
    }

    public async Task<PostResponse?> CreatePost(CreatePostRequest createPost)
    {
        var result = _createPostValidator.Validate(createPost);
        if (!result.IsValid)
        {
            _logger.LogError("CreatePostRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        Post post = _mapper.Map<Post>(createPost);
        post.PublishDate = DateTime.Now;
        await _unitOfWork.GenericRepository.AddAsync(post);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PostResponse>(post);
    }

    public async Task<PostResponse?> UpdatePost(UpdatePostRequest updatePost)
    {
        var result = _updatePostValidator.Validate(updatePost);
        if (!result.IsValid)
        {
            _logger.LogError("UpdatePostRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

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

        _logger.LogError("Object with id {id} doesn't exist", id);
        return null;
    }
}