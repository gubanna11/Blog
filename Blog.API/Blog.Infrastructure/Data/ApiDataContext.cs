using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Data;

public sealed class ApiDataContext : IdentityDbContext<User>
{
    public ApiDataContext(DbContextOptions<ApiDataContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Comment> Comments { get; set; }
    
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.EnableSensitiveDataLogging();
    //}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        builder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        builder.Entity<Category>()
            .HasMany(c => c.Posts)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

        builder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(p => p.PostId);

        base.OnModelCreating(builder);
    }

}
