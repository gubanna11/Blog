using System;

namespace Blog.Core.Contracts.Controllers.Categories;

public sealed record UpdateCategoryRequest(Guid CategoryId, string Name);