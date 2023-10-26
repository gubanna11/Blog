using System.Collections.Generic;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetCommentsQuery : IRequest<IEnumerable<Comment>>;