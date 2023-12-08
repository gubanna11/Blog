using Blog.Core.Contracts.Controllers.Posts;
using MediatR;
using System.Collections.Generic;

namespace Blog.Core.MediatR.Queries.Posts;

public sealed record GetPostsQuery : IRequest<IEnumerable<PostResponse>>;