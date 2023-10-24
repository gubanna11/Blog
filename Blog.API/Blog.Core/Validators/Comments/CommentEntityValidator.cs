using Blog.Core.Entities;
using FluentValidation;

namespace Blog.Core.Validators.Comments;

public class CommentEntityValidator : AbstractValidator<Comment>
{
    public CommentEntityValidator()
    {
        RuleFor(c => c.CommentId)
            .NotEmpty();

        RuleFor(c => c.Content)
            .NotEmpty()
            .MaximumLength(50000);

        RuleFor(p => p.PostId)
            .NotEmpty();

        RuleFor(p => p.UserId)
            .NotEmpty();

        RuleFor(p => p.PublishDate)
            .NotNull();
    }
}
