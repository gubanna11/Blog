using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Entities;

public sealed class User : IdentityUser
{
    public IEnumerable<Post>? Posts { get; set; }
    public IEnumerable<Comment>? Comments { get; set; }
}