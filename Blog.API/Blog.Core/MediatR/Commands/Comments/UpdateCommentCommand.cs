using Blog.Core.Contracts.Controllers.Comments;
using MediatR;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record UpdateCommentCommand(UpdateCommentRequest Comment) : IRequest<CommentResponse?>;