using Microsoft.AspNetCore.Mvc;
using PromptVault.Api.Dtos.Tag;
using PromptVault.Api.Services.Interfaces;

namespace PromptVault.Api.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // GET /api/tags
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tags = await _tagService.GetAllAsync();
        return Ok(tags);
    }

    // GET /api/tags/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tag = await _tagService.GetByIdAsync(id);
        return Ok(tag);
    }

    // POST /api/tags
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTagDto dto)
    {
        var tag = await _tagService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = tag.Id }, tag);
    }

    // DELETE /api/tags/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _tagService.DeleteAsync(id);
        return NoContent();
    }
}
