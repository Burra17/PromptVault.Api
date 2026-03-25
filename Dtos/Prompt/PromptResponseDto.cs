using PromptVault.Api.Dtos.TestResults;

namespace PromptVault.Api.Dtos.Prompt;

public record PromptResponseDto(
    int Id,
    string Text,
    string Category,
    string AuthorNote,
    bool IsPublic,
    DateTime CreatedAt,
    List<string> Tags,
    List<TestResultResponseDto> TestResults
);
