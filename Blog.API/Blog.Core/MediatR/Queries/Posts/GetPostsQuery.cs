using System.Collections.Generic;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Posts;

public sealed record GetPostsQuery : IRequest<IEnumerable<Post>>;