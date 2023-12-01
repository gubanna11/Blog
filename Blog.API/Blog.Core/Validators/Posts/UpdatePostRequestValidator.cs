using Blog.Core.Contracts.Controllers.Posts;
using FluentValidation;

namespace Blog.Core.Validators.Posts;

public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostRequestValidator()
    {
        RuleFor(p => p.PostId)
            .NotEmpty();

        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(p => p.Content)
            .NotEmpty()
            .MinimumLength(50)
            .MaximumLength(120000);

        RuleFor(p => p.UserId)
            .NotEmpty();

        RuleFor(p => p.IsActive)
            .NotEmpty();

        RuleFor(p => p.CategoryId)
            .NotEmpty();
    }
}
