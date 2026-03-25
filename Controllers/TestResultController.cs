using Microsoft.AspNetCore.Mvc;
using PromptVault.Api.Dtos.TestResults;
using PromptVault.Api.Services.Interfaces;

namespace PromptVault.Api.Controllers;

[ApiController]
[Route("api/testresults")]
public class TestResultController : ControllerBase
{
    private readonly ITestResultService _testResultService;

    public TestResultController(ITestResultService testResultService)
    {
        _testResultService = testResultService;
    }

    // GET /api/testresults/prompt/{promptId}
    [HttpGet("prompt/{promptId}")]
    public async Task<IActionResult> GetByPromptId(int promptId)
    {
        var results = await _testResultService.GetByPromptIdAsync(promptId);
        return Ok(results);
    }

    // POST /api/testresults
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTestResultDto dto)
    {
        var result = await _testResultService.CreateAsync(dto);
        return Created("", result);
    }

    // DELETE /api/testresults/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _testResultService.DeleteAsync(id);
        return NoContent();
    }
}
