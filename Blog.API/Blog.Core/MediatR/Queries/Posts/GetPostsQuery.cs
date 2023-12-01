using Blog.Core.ResponseDtos;
using MediatR;
using System.Collections.Generic;

namespace Blog.Core.MediatR.Queries.Posts;

public sealed record GetPostsQuery : IRequest<IEnumerable<PostResponse>>;