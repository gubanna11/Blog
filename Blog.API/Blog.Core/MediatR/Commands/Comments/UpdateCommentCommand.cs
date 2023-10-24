using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record UpdateCommentCommand(UpdateCommentRequest Comment) : IRequest<Comment?>;