using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Entities;
using Blog.Core.ResponseDtos;
using Mapster;

namespace Blog.Infrastructure.Mapster;

public sealed class MapsterRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Category, CategoryResponse>();
        config.ForType<Comment, CommentResponse>();

        config.ForType<CreateCategoryRequest, Category>();
        config.ForType<UpdateCategoryRequest, Category>();

        config.ForType<CreateCommentRequest, Comment>();
        config.ForType<UpdateCommentRequest, Comment>();

        config.ForType<CreatePostRequest, Post>();
        config.ForType<UpdatePostRequest, Post>();
    }
}