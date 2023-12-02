using Blog.Core.Contracts.Controllers.Posts;
using FluentValidation;

namespace Blog.Core.Validators.Posts;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(p => p.Content)
            .MinimumLength(10)
            .MaximumLength(120000);

        RuleFor(p => p.UserId)
            .NotEmpty();

        RuleFor(p => p.IsActive)
            .NotEmpty();

        RuleFor(p => p.CategoryId)
            .NotEmpty();
    }
}
