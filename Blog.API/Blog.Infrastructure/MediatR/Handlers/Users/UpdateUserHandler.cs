using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Users;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Users;

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, User?>
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.UpdateUser(request.User);

        return user;
    }
}