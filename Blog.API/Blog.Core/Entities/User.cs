using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities;

public sealed class User : IdentityUser
{
    public List<Post> Posts { get; set; }
    public List<Comment> Comments { get; set; }
}
