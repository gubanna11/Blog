using System;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Posts;

public sealed record GetPostByIdQuery(Guid Id) : IRequest<Post?>;