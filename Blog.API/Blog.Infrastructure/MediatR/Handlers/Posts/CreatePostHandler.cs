using Blog.Core.MediatR.Commands.Posts;
using Blog.Core.ResponseDtos;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, PostResponse?>
{
    private readonly IPostService _postService;

    public CreatePostHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<PostResponse?> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var responsePost = await _postService.CreatePost(request.Post);

        return responsePost;
    }
}