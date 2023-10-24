using Blog.Core.Entities;
using FluentValidation;

namespace Blog.Core.Validators.Catrgories;

public sealed class CategoryEntityValidator : AbstractValidator<Category>
{
    public CategoryEntityValidator()
    {
        RuleFor(c => c.CategoryId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
