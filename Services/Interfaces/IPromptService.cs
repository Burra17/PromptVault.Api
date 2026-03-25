using PromptVault.Api.Dtos.Prompt;
using PromptVault.Api.Dtos.TestResults;

namespace PromptVault.Api.Services.Interfaces;

public interface IPromptService
{
    Task<IEnumerable<PromptResponseDto>> GetAllAsync();
    Task<PromptResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<PromptResponseDto>> GetTopRatedAsync();
    Task<PromptResponseDto> CreateAsync(CreatePromptDto dto);
    Task<PromptResponseDto> UpdateAsync(int id, UpdatePromptDto dto);
    Task DeleteAsync(int id);

    // Sends the prompt to OpenAI GPT and saves the response as a TestResult.
    Task<TestResultResponseDto> RunAsync(int id);
}
