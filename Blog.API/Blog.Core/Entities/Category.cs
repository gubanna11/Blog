using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities;

public sealed class Category
{
    public int CategoryId { get; set; }

    [StringLength(200)]
    public string Name { get; set; }

    public List<Post> Posts { get; set; }
}
