using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Users;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Users;

public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    private readonly IUserService _userService;

    public GetUsersHandler(IUserService userService)
    {
        _userService = userService;
    }

    public Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userService.GetUsers();

        return Task.FromResult(users);
    }
}