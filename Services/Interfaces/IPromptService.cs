using PromptVault.Api.Dtos.Prompt;

namespace PromptVault.Api.Services.Interfaces;

public interface IPromptService
{
    Task<IEnumerable<PromptResponseDto>> GetAllAsync();
    Task<PromptResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<PromptResponseDto>> GetTopRatedAsync();
    Task<PromptResponseDto> CreateAsync(CreatePromptDto dto);
    Task<PromptResponseDto> UpdateAsync(int id, UpdatePromptDto dto);
    Task DeleteAsync(int id);
}
