using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Posts;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class DeletePostHandler : IRequestHandler<DeletePostCommand, Post?>
{
    private readonly IPostService _postService;

    public DeletePostHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<Post?> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postService.DeletePost(request.Id);

        return post;
    }
}