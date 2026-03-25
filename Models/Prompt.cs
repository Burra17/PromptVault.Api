namespace PromptVault.Api.Models
{
    public class Prompt
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string AuthorNote { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties för relationerna
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
        public ICollection<PromptTag> PromptTags { get; set; } = new List<PromptTag>();
    }
}