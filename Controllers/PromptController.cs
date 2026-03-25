using Microsoft.AspNetCore.Mvc;
using PromptVault.Api.Dtos.Prompt;
using PromptVault.Api.Services.Interfaces;

namespace PromptVault.Api.Controllers;

[ApiController]
[Route("api/prompts")]
public class PromptController : ControllerBase
{
    private readonly IPromptService _promptService;

    public PromptController(IPromptService promptService)
    {
        _promptService = promptService;
    }

    // GET /api/prompts
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var prompts = await _promptService.GetAllAsync();
        return Ok(prompts);
    }

    // GET /api/prompts/top-rated — must be defined before {id} to avoid route conflict
    [HttpGet("top-rated")]
    public async Task<IActionResult> GetTopRated()
    {
        var prompts = await _promptService.GetTopRatedAsync();
        return Ok(prompts);
    }

    // GET /api/prompts/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var prompt = await _promptService.GetByIdAsync(id);
        return Ok(prompt);
    }

    // POST /api/prompts
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePromptDto dto)
    {
        var prompt = await _promptService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = prompt.Id }, prompt);
    }

    // PUT /api/prompts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePromptDto dto)
    {
        var prompt = await _promptService.UpdateAsync(id, dto);
        return Ok(prompt);
    }

    // POST /api/prompts/{id}/run — sends prompt to OpenAI and saves the response as a TestResult
    [HttpPost("{id}/run")]
    public async Task<IActionResult> Run(int id)
    {
        var result = await _promptService.RunAsync(id);
        return Created("", result);
    }

    // DELETE /api/prompts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _promptService.DeleteAsync(id);
        return NoContent();
    }
}
