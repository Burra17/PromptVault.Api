using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromptVault.Api.Database;
using PromptVault.Api.Dtos.TestResults;
using PromptVault.Api.Models;
using PromptVault.Api.Services.Interfaces;

namespace PromptVault.Api.Services;

// Implements ITestResultService — handles CRUD for test results (AI model outputs + ratings).
// Each TestResult belongs to a Prompt via PromptId (one-to-many relationship).
public class TestResultService : ITestResultService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTestResultDto> _validator;

    public TestResultService(AppDbContext context, IMapper mapper, IValidator<CreateTestResultDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // Returns all test results for a specific prompt, filtered by PromptId.
    public async Task<IEnumerable<TestResultResponseDto>> GetByPromptIdAsync(int promptId)
    {
        var results = await _context.Results
            .Where(r => r.PromptId == promptId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TestResultResponseDto>>(results);
    }

    // Creates a new test result. Validates input and verifies the parent prompt exists
    // before saving — prevents orphaned test results pointing to non-existent prompts.
    public async Task<TestResultResponseDto> CreateAsync(CreateTestResultDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Verify that the parent prompt exists before creating a test result
        var promptExists = await _context.Prompts.AnyAsync(p => p.Id == dto.PromptId);
        if (!promptExists)
            throw new KeyNotFoundException($"Prompt with ID {dto.PromptId} was not found.");

        var testResult = _mapper.Map<TestResult>(dto);
        testResult.CreatedAt = DateTime.UtcNow;

        _context.Results.Add(testResult);
        await _context.SaveChangesAsync();

        return _mapper.Map<TestResultResponseDto>(testResult);
    }

    // Deletes a test result by ID. Does not affect the parent prompt.
    public async Task DeleteAsync(int id)
    {
        var result = await _context.Results.FindAsync(id);

        if (result == null)
            throw new KeyNotFoundException($"TestResult with ID {id} was not found.");

        _context.Results.Remove(result);
        await _context.SaveChangesAsync();
    }
}
