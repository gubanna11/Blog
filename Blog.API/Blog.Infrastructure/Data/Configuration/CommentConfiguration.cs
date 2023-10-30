using Blog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Blog.Infrastructure.Data.Configuration;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    private readonly Guid[] _postsGuids;
    private readonly string _userId;

    public CommentConfiguration(Guid[] postsGuids, Guid userId)
    {
        _postsGuids = postsGuids;
        _userId = userId.ToString();
    }

    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        Guid[] commentsGuids = { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), };

        Comment[] comments =
        {
            new()
            {
                CommentId = commentsGuids[0],
                Content = "Good job!",
                PostId = _postsGuids[0],
                UserId = _userId,
                PublishDate = DateTime.Now,
                ParentCommentId = null,
            },
            new()
            {
                CommentId = commentsGuids[1],
                Content = "That's fine.",
                PostId = _postsGuids[1],
                UserId = _userId,
                PublishDate = DateTime.Now,
                ParentCommentId = null,
            },
            new()
            {
                CommentId = commentsGuids[2],
                Content = "AWFUL!",
                PostId = _postsGuids[2],
                UserId = _userId,
                PublishDate = DateTime.Now,
                ParentCommentId = null,
            },
            new()
            {
                CommentId = commentsGuids[3],
                Content = "YEAH!",
                PostId = _postsGuids[2],
                UserId = _userId,
                PublishDate = DateTime.Now,
                ParentCommentId = commentsGuids[2],
            },
            new()
            {
                CommentId = commentsGuids[4],
                Content = "TOTALLY!",
                PostId = _postsGuids[2],
                UserId = _userId,
                PublishDate = DateTime.Now,
                ParentCommentId = commentsGuids[2],
            },
        };

        builder.HasData(comments);
    }
}
