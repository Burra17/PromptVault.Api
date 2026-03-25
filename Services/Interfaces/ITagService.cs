using PromptVault.Api.Dtos.Tag;

namespace PromptVault.Api.Services.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagResponseDto>> GetAllAsync();
    Task<TagResponseDto> GetByIdAsync(int id);
    Task<TagResponseDto> CreateAsync(CreateTagDto dto);
    Task DeleteAsync(int id);
}
