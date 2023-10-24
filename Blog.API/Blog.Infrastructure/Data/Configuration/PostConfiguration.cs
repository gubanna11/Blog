using Blog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Blog.Infrastructure.Data.Configuration;

internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    private readonly Guid[] _categoryGuids;
    private readonly Guid[] _postGuids;
    private readonly string _userId;

    public PostConfiguration(Guid[] categoryGuids, Guid[] postGuids, Guid userGuid)
    {
        _categoryGuids = categoryGuids;
        _postGuids = postGuids;
        _userId = userGuid.ToString();
    }

    public void Configure(EntityTypeBuilder<Post> builder)
    {
        Post[] posts =
        {
            new()
            {
                PostId = _postGuids[0],
                Title = "Sample Post 1",
                Content = "This is the content of the first sample post. It should be at least 50 characters long.",
                PublishDate = DateTime.Now,
                IsActive = true,
                CategoryId = _categoryGuids[0],
                UserId = _userId,
            },
            new()
            {
                PostId = _postGuids[1],
                Title = "Sample Post 2",
                Content = "This is the content of the second sample post. It should be at least 50 characters long.",
                PublishDate = DateTime.Now,
                IsActive = true,
                CategoryId = _categoryGuids[0],
                UserId = _userId,
            },
            new()
            {
                PostId = _postGuids[2],
                Title = "Sample Post 3",
                Content = "This is the content of the third sample post. It should be at least 50 characters long.",
                PublishDate = DateTime.Now,
                IsActive = true,
                CategoryId = _categoryGuids[1],
                UserId = _userId,
            },
            new()
            {
                PostId = _postGuids[3],
                Title = "Sample Post 4",
                Content = "This is the content of the fourth sample post. It should be at least 50 characters long.",
                PublishDate = DateTime.Now,
                IsActive = true,
                CategoryId = _categoryGuids[1],
                UserId = _userId,
            },
            new()
            {
                PostId = _postGuids[4],
                Title = "Sample Post 5",
                Content = "This is the content of the fifth sample post. It should be at least 50 characters long.",
                PublishDate = DateTime.Now,
                IsActive = true,
                CategoryId = _categoryGuids[2],
                UserId = _userId,
            }
        };

        builder.HasData(posts);
    }
}
