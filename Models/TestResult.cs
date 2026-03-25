namespace PromptVault.Api.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public string Output { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string ModelUsed { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key & Navigation Property
        public int PromptId { get; set; }
        public Prompt Prompt { get; set; } = null!;
    }
}