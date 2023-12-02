using Blog.Core.Contracts.ResponseDtos;
using MediatR;
using System;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetCommentByIdQuery(Guid Id) : IRequest<CommentResponse?>;