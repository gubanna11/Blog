using Blog.Core.Contracts.Controllers.Comments;
using FluentValidation;

namespace Blog.Core.Validators.Comments;

public sealed class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(c => c.Content)
            .NotEmpty()
            .MaximumLength(50000);

        RuleFor(p => p.PostId)
            .NotEmpty();

        RuleFor(p => p.UserId)
            .NotEmpty();
    }
}
