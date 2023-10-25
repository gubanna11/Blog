using Blog.Core.Contracts.Controllers.Users;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Users;

public sealed record CreateUserCommand(RegisterRequest RegisterRequest) : IRequest<User>;