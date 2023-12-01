using Blog.Core.MediatR.Commands.Posts;
using Blog.Core.ResponseDtos;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class DeletePostHandler : IRequestHandler<DeletePostCommand, PostResponse?>
{
    private readonly IPostService _postService;

    public DeletePostHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<PostResponse?> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postService.DeletePost(request.Id);

        return post;
    }
}