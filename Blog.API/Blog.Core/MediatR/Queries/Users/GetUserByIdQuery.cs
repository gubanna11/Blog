using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Users;

public sealed record GetUserByIdQuery(string Id) : IRequest<User?>;