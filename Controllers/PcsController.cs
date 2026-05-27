namespace Apbdtask7.Controllers;
using Apbdtask7.DTOs;
using Apbdtask7.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/pcs")]
public class PcsController : ControllerBase
{
    private readonly IPcService _service;

    public PcsController(IPcService pcService)
    {
        _service = pcService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var computers = await _service.GetAllPcsAsync();
        return Ok(computers);
    }

    [HttpGet("{id:int}/components")]
    public async Task<IActionResult> GetComponents(int id)
    {
        var systemData = await _service.GetPcComponentsAsync(id);
        if (systemData is null)
            return NotFound($"Computer with id {id} was not found.");
        return Ok(systemData);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePcRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newEntry = await _service.CreatePcAsync(request);
        return CreatedAtAction(nameof(GetComponents), new { id = newEntry.Id }, newEntry);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePcRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isUpdated = await _service.UpdatePcAsync(id, request);
        if (!isUpdated)
            return NotFound($"Computer with id {id} was not found.");
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _service.DeletePcAsync(id);
        if (!isDeleted)
            return NotFound($"Computer with id {id} was not found.");
        return NoContent();
    }
}