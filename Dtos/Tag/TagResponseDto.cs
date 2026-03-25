namespace PromptVault.Api.Dtos.Tag;

// Class instead of record — AutoMapper needs a parameterless constructor to map properties.
public class TagResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
