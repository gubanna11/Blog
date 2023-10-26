using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class UpdatePostHandler : IRequestHandler<UpdatePostCommand, Post?>
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public UpdatePostHandler(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    public async Task<Post?> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = _mapper.Map<Post>(request.Post);
        var responsePost = await _postService.UpdatePost(post);

        return responsePost;
    }
}