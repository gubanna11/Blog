using Blog.Core.Contracts.Controllers.Comments;
using FluentValidation;

namespace Blog.Core.Validators.Comments;

public sealed class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
{
    public UpdateCommentRequestValidator()
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
    }
}
