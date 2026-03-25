using AutoMapper;
using PromptVault.Api.Dtos.Prompt;
using PromptVault.Api.Dtos.Tag;
using PromptVault.Api.Dtos.TestResults;
using PromptVault.Api.Models;

namespace PromptVault.Api.AutoMapper;

public class AutoMapperProfile : Profile // Inherits from automapper baseclass
{
    public AutoMapperProfile()
    {
        // Maps Prompt to PromptResponseDto. Properties with matching names (Id, Text, Category, etc.)
        // are mapped automatically. Tags requires a custom rule since the model uses PromptTags (join table)
        // while the DTO expects a simple list of tag names.
        CreateMap<Prompt, PromptResponseDto>()
            .ForMember(dest => dest.Tags,
                opt => opt.MapFrom(src => src.PromptTags.Select(pt => pt.Tag.Name).ToList()));

        CreateMap<CreatePromptDto, Prompt>();
        CreateMap<UpdatePromptDto, Prompt>();

        // Tag mappings
        CreateMap<Tag, TagResponseDto>();
        CreateMap<CreateTagDto, Tag>();

        // TestResult mappings
        CreateMap<TestResult, TestResultResponseDto>();
        CreateMap<CreateTestResultDto, TestResult>();
    }
}
