using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record CreateCommentCommand(CreateCommentRequest Comment) : IRequest<Comment>;