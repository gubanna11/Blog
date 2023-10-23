using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Blog.Core.Entities;

public sealed class User : IdentityUser
{
    public IEnumerable<Post>? Posts { get; set; }
    public IEnumerable<Comment>? Comments { get; set; }
}
