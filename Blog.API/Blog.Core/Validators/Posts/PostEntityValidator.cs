using Blog.Core.Entities;
using FluentValidation;

namespace Blog.Core.Validators.Posts;

public class PostEntityValidator : AbstractValidator<Post>
{
    public PostEntityValidator()
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

        RuleFor(p => p.PublishDate)
            .NotNull();

        RuleFor(p => p.IsActive)
            .NotEmpty();

        RuleFor(p => p.CategoryId)
            .NotEmpty();
    }
}
