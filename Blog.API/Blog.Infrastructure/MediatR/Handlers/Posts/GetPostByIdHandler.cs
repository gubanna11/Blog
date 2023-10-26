using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, Post?>
{
    private readonly IPostService _postService;

    public GetPostByIdHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<Post?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postService.GetPostById(request.Id);

        return post;
    }
}