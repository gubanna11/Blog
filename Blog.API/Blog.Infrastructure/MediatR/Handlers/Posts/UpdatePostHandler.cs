using Blog.Core.MediatR.Commands.Posts;
using Blog.Core.ResponseDtos;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class UpdatePostHandler : IRequestHandler<UpdatePostCommand, PostResponse?>
{
    private readonly IPostService _postService;

    public UpdatePostHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<PostResponse?> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var responsePost = await _postService.UpdatePost(request.Post);

        return responsePost;
    }
}