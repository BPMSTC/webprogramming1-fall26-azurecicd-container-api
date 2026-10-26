using baseballfun.Dtos;
using baseballfun.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace baseballfun.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController(IGameRepository games) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAll([FromQuery] string? opponent, CancellationToken ct)
    {
        var result = await games.GetAllAsync(opponent, ct);
        return Ok(result.Select(g => g.ToResponse()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameResponse>> GetById(int id, CancellationToken ct)
    {
        var game = await games.GetByIdAsync(id, ct);
        return game is null ? NotFound() : Ok(game.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<GameResponse>> Create(GameRequest request, CancellationToken ct)
    {
        var created = await games.AddAsync(request.ToEntity(), ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GameResponse>> Update(int id, GameRequest request, CancellationToken ct)
    {
        var entity = request.ToEntity(id);
        return await games.UpdateAsync(entity, ct) ? Ok(entity.ToResponse()) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) =>
        await games.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
