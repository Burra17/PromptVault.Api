namespace PromptVault.Api.Configuration;

// Binds to the "OpenAI" section in appsettings.json / user secrets.
public class OpenAiSettings
{
    public string ApiKey { get; set; } = string.Empty;
}
