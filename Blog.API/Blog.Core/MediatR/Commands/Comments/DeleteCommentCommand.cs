using Blog.Core.Contracts.Controllers.Comments;
using MediatR;
using System;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record DeleteCommentCommand(Guid Id) : IRequest<CommentResponse?>;