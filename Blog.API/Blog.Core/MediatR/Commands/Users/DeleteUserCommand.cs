using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Users;

public sealed record DeleteUserCommand(string Id) : IRequest<User?>;