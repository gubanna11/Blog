using Blog.Core.Contracts.Controllers.Categories;
using FluentValidation;

namespace Blog.Core.Validators.Categories
{
    public sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty();

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
