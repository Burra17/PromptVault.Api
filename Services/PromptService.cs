using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromptVault.Api.Database;
using PromptVault.Api.Dtos.Prompt;
using PromptVault.Api.Models;
using PromptVault.Api.Services.Interfaces;

namespace PromptVault.Api.Services;

// Implements IPromptService — contains all business logic for Prompt CRUD operations.
// Dependencies are injected via constructor: database context, AutoMapper, and FluentValidation validators.
public class PromptService : IPromptService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePromptDto> _createValidator;
    private readonly IValidator<UpdatePromptDto> _updateValidator;

    // DI container automatically provides these dependencies (registered in Program.cs)
    public PromptService(
        AppDbContext context,
        IMapper mapper,
        IValidator<CreatePromptDto> createValidator,
        IValidator<UpdatePromptDto> updateValidator)
    {
        _context = context;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // Fetches all prompts with their related tags and test results.
    // Include + ThenInclude is required for AutoMapper to map PromptTags → tag name strings.
    public async Task<IEnumerable<PromptResponseDto>> GetAllAsync()
    {
        var prompts = await _context.Prompts
            .Include(p => p.PromptTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.TestResults)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PromptResponseDto>>(prompts);
    }

    // Fetches a single prompt by ID. Throws KeyNotFoundException if not found.
    public async Task<PromptResponseDto> GetByIdAsync(int id)
    {
        var prompt = await _context.Prompts
            .Include(p => p.PromptTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.TestResults)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prompt == null)
            throw new KeyNotFoundException($"Prompt with ID {id} was not found.");

        return _mapper.Map<PromptResponseDto>(prompt);
    }

    // Returns top 10 public prompts sorted by average test result rating (highest first).
    // Only includes prompts that have at least one test result.
    public async Task<IEnumerable<PromptResponseDto>> GetTopRatedAsync()
    {
        var prompts = await _context.Prompts
            .Where(p => p.IsPublic)
            .Include(p => p.PromptTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.TestResults)
            .Where(p => p.TestResults.Any())
            .OrderByDescending(p => p.TestResults.Average(t => t.Rating))
            .Take(10)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PromptResponseDto>>(prompts);
    }

    // Creates a new prompt. Validates input, maps DTO to model, and handles tag creation.
    // Tags are resolved by name: existing tags are reused, new ones are created automatically.
    public async Task<PromptResponseDto> CreateAsync(CreatePromptDto dto)
    {
        // Validate input using FluentValidation rules (defined in CreatePromptValidator)
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Map DTO to model (Text, Category, AuthorNote, IsPublic). TagNames is ignored by AutoMapper.
        var prompt = _mapper.Map<Prompt>(dto);
        prompt.CreatedAt = DateTime.UtcNow;

        // Handle tags: find existing by name or create new ones, then link via PromptTag join table
        if (dto.TagNames != null && dto.TagNames.Any())
        {
            foreach (var tagName in dto.TagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync(); // Save to get the generated Tag.Id
                }

                prompt.PromptTags.Add(new PromptTag { TagId = tag.Id });
            }
        }

        _context.Prompts.Add(prompt);
        await _context.SaveChangesAsync();

        // Re-fetch with all includes to return a complete DTO with tags and test results
        return await GetByIdAsync(prompt.Id);
    }

    // Updates an existing prompt. Validates input, updates fields, and replaces all tag connections.
    public async Task<PromptResponseDto> UpdateAsync(int id, UpdatePromptDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Include PromptTags so we can remove old tag connections
        var prompt = await _context.Prompts
            .Include(p => p.PromptTags)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prompt == null)
            throw new KeyNotFoundException($"Prompt with ID {id} was not found.");

        // Update fields directly on the tracked entity (EF Core detects changes automatically)
        prompt.Text = dto.Text;
        prompt.Category = dto.Category;
        prompt.AuthorNote = dto.AuthorNote;
        prompt.IsPublic = dto.IsPublic;

        // Replace all tag connections: remove old ones, then add new ones
        if (dto.TagNames != null)
        {
            _context.PromptTags.RemoveRange(prompt.PromptTags);

            foreach (var tagName in dto.TagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }

                prompt.PromptTags.Add(new PromptTag { TagId = tag.Id });
            }
        }

        await _context.SaveChangesAsync();

        return await GetByIdAsync(prompt.Id);
    }

    // Deletes a prompt by ID. Throws KeyNotFoundException if not found.
    // Related PromptTags and TestResults are deleted automatically via cascade delete.
    public async Task DeleteAsync(int id)
    {
        var prompt = await _context.Prompts.FindAsync(id);

        if (prompt == null)
            throw new KeyNotFoundException($"Prompt with ID {id} was not found.");

        _context.Prompts.Remove(prompt);
        await _context.SaveChangesAsync();
    }
}
