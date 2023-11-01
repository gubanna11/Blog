using Blog.Core.Contracts.Controllers.Categories;
using FluentValidation;

namespace Blog.Core.Validators.Categories;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
