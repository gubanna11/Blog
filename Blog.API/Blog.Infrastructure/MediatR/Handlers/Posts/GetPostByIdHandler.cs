using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Queries.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostResponse?>
{
    private readonly IPostService _postService;

    public GetPostByIdHandler(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<PostResponse?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postService.GetPostById(request.Id);

        return post;
    }
}