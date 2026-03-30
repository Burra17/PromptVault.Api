namespace PromptVault.Api.Dtos.TestResults;

public record CreateTestResultDto(
    string Output,
    int Rating,
    string ModelUsed,
    int PromptId
);
