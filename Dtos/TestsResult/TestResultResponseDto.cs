namespace PromptVault.Api.Dtos.TestResults;

// Class instead of record — AutoMapper needs a parameterless constructor to map properties.
public class TestResultResponseDto
{
    public int Id { get; set; }
    public string Output { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string ModelUsed { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int PromptId { get; set; }
}
