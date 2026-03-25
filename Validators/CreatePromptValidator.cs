using FluentValidation;
using PromptVault.Api.Dtos.Prompt;

namespace PromptVault.Api.Validators;

public class CreatePromptValidator : AbstractValidator<CreatePromptDto>
{
    public CreatePromptValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MaximumLength(5000).WithMessage("Text must not exceed 5000 characters.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

        RuleFor(x => x.AuthorNote)
            .MaximumLength(500).WithMessage("AuthorNote must not exceed 500 characters.");

        RuleForEach(x => x.TagNames)
            .NotEmpty().WithMessage("Tag name must not be empty.")
            .MaximumLength(50).WithMessage("Tag name must not exceed 50 characters.");
    }
}
