using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Posts;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class UpdatePostHandler : IRequestHandler<UpdatePostCommand, Post?>
{
    private readonly IPostService _postService;

    public UpdatePostHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<Post?> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postService.UpdatePost(request.Post);

        return post;
    }
}