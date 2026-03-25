namespace PromptVault.Api.Dtos.Prompt;

public record UpdatePromptDto(
    string Text,
    string Category,
    string AuthorNote = "",
    bool IsPublic = false,
    List<string>? TagNames = null
);
