using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities;

public sealed class Comment
{
    [Key]
    public int CommentId { get; set; }

    [StringLength(50000)]
    public string Content { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public DateTime PublishDate { get; set; }

    public int? ParentCommentId { get; set; }
}
