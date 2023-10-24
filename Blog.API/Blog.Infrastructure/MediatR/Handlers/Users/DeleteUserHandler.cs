using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Users;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Users;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, User?>
{
    private readonly IUserService _userService;

    public DeleteUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<User?> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.DeleteUser(request.Id);

        return user;
    }
}