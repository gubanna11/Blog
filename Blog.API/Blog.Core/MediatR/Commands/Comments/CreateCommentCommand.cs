using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.ResponseDtos;
using MediatR;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record CreateCommentCommand(CreateCommentRequest Comment) : IRequest<CommentResponse?>;