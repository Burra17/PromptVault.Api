using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromptVault.Api.Database;
using PromptVault.Api.Dtos.Tag;
using PromptVault.Api.Models;
using PromptVault.Api.Services.Interfaces;

namespace PromptVault.Api.Services;

// Implements ITagService — handles CRUD operations for tags.
// Simpler than PromptService since Tag only has Id and Name.
public class TagService : ITagService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTagDto> _validator;

    public TagService(AppDbContext context, IMapper mapper, IValidator<CreateTagDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // Returns all tags. No Include needed since Tag has no navigation properties we need.
    public async Task<IEnumerable<TagResponseDto>> GetAllAsync()
    {
        var tags = await _context.Tags.ToListAsync();
        return _mapper.Map<IEnumerable<TagResponseDto>>(tags);
    }

    // Finds a tag by ID. FindAsync uses primary key lookup (faster than FirstOrDefault for PK).
    public async Task<TagResponseDto> GetByIdAsync(int id)
    {
        var tag = await _context.Tags.FindAsync(id);

        if (tag == null)
            throw new KeyNotFoundException($"Tag with ID {id} was not found.");

        return _mapper.Map<TagResponseDto>(tag);
    }

    // Validates input, maps DTO to Tag model, saves and returns the created tag.
    public async Task<TagResponseDto> CreateAsync(CreateTagDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var tag = _mapper.Map<Tag>(dto);
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        return _mapper.Map<TagResponseDto>(tag);
    }

    // Deletes a tag by ID. Related PromptTag connections are removed via cascade delete.
    public async Task DeleteAsync(int id)
    {
        var tag = await _context.Tags.FindAsync(id);

        if (tag == null)
            throw new KeyNotFoundException($"Tag with ID {id} was not found.");

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }
}
