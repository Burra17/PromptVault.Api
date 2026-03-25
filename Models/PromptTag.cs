namespace PromptVault.Api.Models
{
    public class PromptTag
    {
        public int PromptId { get; set; }
        public Prompt Prompt { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}