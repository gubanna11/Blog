using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class CreateCommentHandler : IRequestHandler<CreateCommentCommand, Comment>
{
    private readonly ICommentService _commentService;
    private readonly IMapper _mapper;

    public CreateCommentHandler(ICommentService commentService, IMapper mapper)
    {
        _commentService = commentService;
        _mapper = mapper;
    }

    public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = _mapper.Map<Comment>(request.Comment);
        var responseComment = await _commentService.CreateComment(comment);

        return responseComment;
    }
}