using Blog.Core.ResponseDtos;
using MediatR;
using System;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record DeleteCommentCommand(Guid Id) : IRequest<CommentResponse?>;