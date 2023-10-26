using System.Collections.Generic;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Users;

public sealed record GetUsersQuery : IRequest<IEnumerable<User>>;