using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.Contracts.ResponseDtos;
using MediatR;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record CreateCommentCommand(CreateCommentRequest Comment) : IRequest<CommentResponse?>;