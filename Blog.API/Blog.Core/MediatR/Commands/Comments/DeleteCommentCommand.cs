using System;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Comments;

public sealed record DeleteCommentCommand(Guid Id) : IRequest<Comment?>;