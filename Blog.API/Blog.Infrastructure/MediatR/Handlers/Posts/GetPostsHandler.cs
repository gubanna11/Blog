using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Posts;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class GetPostsHandler : IRequestHandler<GetPostsQuery, IEnumerable<Post>>
{
    private readonly IPostService _postService;

    public GetPostsHandler(IPostService postService)
    {
        _postService = postService;
    }

    public Task<IEnumerable<Post>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = _postService.GetPosts();

        return Task.FromResult(posts);
    }
}