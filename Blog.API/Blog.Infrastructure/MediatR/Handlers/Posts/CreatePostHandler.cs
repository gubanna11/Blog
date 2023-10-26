using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, Post>
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public CreatePostHandler(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = _mapper.Map<Post>(request.Post);
        var responsePost = await _postService.CreatePost(post);

        return responsePost;
    }
}