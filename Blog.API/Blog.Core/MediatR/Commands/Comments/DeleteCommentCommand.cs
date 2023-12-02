using Blog.Core.Contracts.ResponseDtos;
using MediatR;
using System;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record DeleteCommentCommand(Guid Id) : IRequest<CommentResponse?>;