using System;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetCommentByIdQuery(Guid Id) : IRequest<Comment?>;