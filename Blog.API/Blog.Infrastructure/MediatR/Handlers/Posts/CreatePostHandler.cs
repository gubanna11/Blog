using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Posts;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, Post>
{
    private readonly IPostService _postService;

    public CreatePostHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postService.CreatePost(request.Post);

        return post;
    }
}