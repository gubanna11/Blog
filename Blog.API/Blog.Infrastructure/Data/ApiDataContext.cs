using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    }

}
