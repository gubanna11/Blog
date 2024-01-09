using System;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    private const string _adminUser = "Admin";
    private const string _adminPassword = "Secret123$";
    private const string _adminEmail = "admin@example.com";
    private readonly Guid _adminId;

    public UserConfiguration(Guid adminId)
    {
        _adminId = adminId;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        PasswordHasher<User> hasher = new();

        User user = new()
        {
            Id = _adminId.ToString(),
            UserName = _adminUser,
            NormalizedUserName = _adminUser.ToUpper(),
            Email = _adminEmail,
            NormalizedEmail = _adminEmail.ToUpper(),
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = false,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        user.PasswordHash = hasher.HashPassword(user, _adminPassword);

        builder.HasData(user);
    }
}