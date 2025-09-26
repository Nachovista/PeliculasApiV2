using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Data; // tu namespace
using PeliculasApi.Models; // tu namespace

[ApiController]
[Route("api/[controller]")]
public class PeliculasController : ControllerBase
{
    private readonly AppDbContext _db;
    public PeliculasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Peliculas>>> GetAll()
        => await _db.Peliculas.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Peliculas>> GetById(int id)
        => await _db.Peliculas.FindAsync(id) is { } p ? Ok(p) : NotFound();

    [HttpPost]
    public async Task<ActionResult<Peliculas>> Create(Peliculas p)
    {
        _db.Peliculas.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Peliculas p)
    {
        if (id != p.Id) return BadRequest();
        _db.Entry(p).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Peliculas.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Peliculas.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}