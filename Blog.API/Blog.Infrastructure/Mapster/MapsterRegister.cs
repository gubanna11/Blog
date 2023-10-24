using Blog.Core.Entities;
using Mapster;

namespace Blog.Infrastructure.Mapster;

public sealed class MapsterRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateCategoryRequest, Category>();
        config.ForType<UpdateCategoryRequest, Category>();

        config.ForType<CreateCommentRequest, Comment>();
        config.ForType<UpdateCommentRequest, Comment>();

        config.ForType<CreatePostRequest, Post>();
        config.ForType<UpdatePostRequest, Post>();
    }
}