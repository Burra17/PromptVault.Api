namespace PromptVault.Api.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<PromptTag> PromptTags { get; set; } = new List<PromptTag>();
    }
}