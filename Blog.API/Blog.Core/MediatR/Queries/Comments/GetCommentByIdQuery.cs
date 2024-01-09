using Blog.Core.Contracts.Controllers.Comments;
using MediatR;
using System;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetCommentByIdQuery(Guid Id) : IRequest<CommentResponse?>;