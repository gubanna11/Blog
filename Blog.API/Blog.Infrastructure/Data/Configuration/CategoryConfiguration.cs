using Blog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Blog.Infrastructure.Data.Configuration;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    private readonly Guid[] _categoryGuids;

    public CategoryConfiguration(Guid[] categoryGuids)
    {
        _categoryGuids = categoryGuids;
    }

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        Category[] categories =
        {
            new()
            {
                CategoryId = _categoryGuids[0],
                Name = "FirstCa",
            },
            new()
            {
                CategoryId = _categoryGuids[1],
                Name = "SecondCa",
            },
            new()
            {
                CategoryId = _categoryGuids[2],
                Name = "ThirdCa",
            },
        };

        builder.HasData(categories);
    }
}
