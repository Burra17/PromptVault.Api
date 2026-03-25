namespace PromptVault.Api.Services.Interfaces;

public interface IOpenAiService
{
    // Sends a prompt to OpenAI GPT and returns the generated response text.
    Task<string> GetCompletionAsync(string prompt);
}
