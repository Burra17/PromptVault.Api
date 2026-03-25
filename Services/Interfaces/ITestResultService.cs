using PromptVault.Api.Dtos.TestResults;

namespace PromptVault.Api.Services.Interfaces;

public interface ITestResultService
{
    Task<IEnumerable<TestResultResponseDto>> GetByPromptIdAsync(int promptId);
    Task<TestResultResponseDto> CreateAsync(CreateTestResultDto dto);
    Task DeleteAsync(int id);
}
