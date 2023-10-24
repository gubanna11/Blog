using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Users;

public sealed record CreateUserCommand(CreateUserRequest User) : IRequest<User>;