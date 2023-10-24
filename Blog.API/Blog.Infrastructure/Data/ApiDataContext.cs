using Blog.Core.Entities;
using Blog.Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.Infrastructure.Data;

public sealed class ApiDataContext : IdentityDbContext<User>
{
    public ApiDataContext(DbContextOptions<ApiDataContext> options) : base(options) { }

    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        Guid[] categoryGuids = { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), };
        Guid[] postGuids = { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), };
        Guid adminId = Guid.NewGuid();

        builder.ApplyConfiguration(new CategoryConfiguration(categoryGuids));
        builder.ApplyConfiguration(new UserConfiguration(adminId));
        builder.ApplyConfiguration(new PostConfiguration(categoryGuids, postGuids, adminId));
        builder.ApplyConfiguration(new CommentConfiguration(postGuids, adminId));
    }
}
