using Blog.Core.Contracts.ResponseDtos;
using MediatR;
using System;

namespace Blog.Core.MediatR.Queries.Posts;

public sealed record GetPostByIdQuery(Guid Id) : IRequest<PostResponse?>;