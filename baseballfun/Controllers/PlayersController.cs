using baseballfun.Dtos;
using baseballfun.Models;
using baseballfun.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace baseballfun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController(IPlayerRepository players) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerResponse>>> GetAll([FromQuery] Position? position, CancellationToken ct)
    {
        var result = await players.GetAllAsync(position, ct);
        return Ok(result.Select(p => p.ToResponse()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlayerResponse>> GetById(int id, CancellationToken ct)
    {
        var player = await players.GetByIdAsync(id, ct);
        return player is null ? NotFound() : Ok(player.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<PlayerResponse>> Create(PlayerRequest request, CancellationToken ct)
    {
        var created = await players.AddAsync(request.ToEntity(), ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlayerResponse>> Update(int id, PlayerRequest request, CancellationToken ct)
    {
        var entity = request.ToEntity(id);
        return await players.UpdateAsync(entity, ct) ? Ok(entity.ToResponse()) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) =>
        await players.DeleteAsync(id, ct) ? NoContent() : NotFound();

    [HttpGet("{id:int}/stats")]
    public async Task<ActionResult<IEnumerable<PlayerStatResponse>>> GetStats(int id, CancellationToken ct)
    {
        var stats = await players.GetStatsAsync(id, ct);
        return stats is null ? NotFound() : Ok(stats.Select(s => s.ToResponse()));
    }

    [HttpPost("{id:int}/stats")]
    public async Task<ActionResult<PlayerStatResponse>> AddStat(int id, PlayerStatRequest request, CancellationToken ct)
    {
        try
        {
            var created = await players.AddStatAsync(id, request.ToEntity(), ct);
            return created is null
                ? NotFound()
                : CreatedAtAction(nameof(GetStats), new { id }, created.ToResponse());
        }
        catch (DuplicateStatException ex)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Duplicate season stats", detail: ex.Message);
        }
    }
}
