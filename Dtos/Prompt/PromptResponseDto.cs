using PromptVault.Api.Dtos.TestResults;

namespace PromptVault.Api.Dtos.Prompt;

// Class instead of record — AutoMapper needs a parameterless constructor to map properties.
public class PromptResponseDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string AuthorNote { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<TestResultResponseDto> TestResults { get; set; } = new();
}
