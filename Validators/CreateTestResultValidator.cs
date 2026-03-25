using FluentValidation;
using PromptVault.Api.Dtos.TestResults;

namespace PromptVault.Api.Validators;

public class CreateTestResultValidator : AbstractValidator<CreateTestResultDto>
{
    public CreateTestResultValidator()
    {
        RuleFor(x => x.Output)
            .NotEmpty().WithMessage("Output is required.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.ModelUsed)
            .NotEmpty().WithMessage("ModelUsed is required.")
            .MaximumLength(100).WithMessage("ModelUsed must not exceed 100 characters.");

        RuleFor(x => x.PromptId)
            .GreaterThan(0).WithMessage("PromptId must be a valid ID.");
    }
}
