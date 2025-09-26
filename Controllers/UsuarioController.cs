using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Data;
using PeliculasApi.Models;

namespace PeliculasApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _ctx;
    public UsuarioController(AppDbContext ctx) => _ctx = ctx;

    // ====== DTOs ======
    public record LoginRequest(string UserOrEmail, string Password);

    public record UsuarioDto(
        int Id,
        string Nombre,
        string Email,
        string? Imagen,
        string Rol,
        string? Contraseña
    );

    // Para aceptar el JSON del cliente que envía "contraseña" (con ñ)
    public record UsuarioRegisterDto(
        [property: JsonPropertyName("nombre")] string Nombre,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("imagen")] string? Imagen,
        [property: JsonPropertyName("rol")] string Rol,
        [property: JsonPropertyName("contraseña")] string Contrasena
    );

    // ====== CRUD ======

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var usuarios = await _ctx.Usuario
            .AsNoTracking()
            .Select(u => new UsuarioDto(u.Id, u.Nombre, u.Email, u.Imagen, u.Rol, u.Contraseña))
            .ToListAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id)
    {
        var u = await _ctx.Usuario.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (u is null) return NotFound();

        return Ok(new UsuarioDto(u.Id, u.Nombre, u.Email, u.Imagen, u.Rol, u.Contraseña));
    }

    [HttpPost] // POST api/Usuario
    public async Task<ActionResult<UsuarioDto>> Create([FromBody] UsuarioDto dto)
    {
        if (dto is null) return BadRequest();

        // Si querés validar duplicados por email:
        // if (await _ctx.Usuario.AnyAsync(x => x.Email == dto.Email))
        //     return Conflict($"Ya existe un usuario con email {dto.Email}");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Imagen = dto.Imagen,
            Rol = string.IsNullOrWhiteSpace(dto.Rol) ? "usuario" : dto.Rol,
            Contraseña = string.IsNullOrWhiteSpace(dto.Contraseña) ? "1234" : dto.Contraseña
        };

        _ctx.Usuario.Add(usuario);
        await _ctx.SaveChangesAsync();

        var result = new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Imagen, usuario.Rol, usuario.Contraseña);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, result);
    }

    [HttpPut("{id}")] // PUT api/Usuario/{id}
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto dto)
    {
        var usuario = await _ctx.Usuario.FindAsync(id);
        if (usuario is null) return NotFound();

        usuario.Nombre = dto.Nombre;
        usuario.Email = dto.Email;
        usuario.Imagen = dto.Imagen;
        usuario.Rol = dto.Rol;

        // Si querés permitir actualizar contraseña cuando venga:
        if (!string.IsNullOrWhiteSpace(dto.Contraseña))
            usuario.Contraseña = dto.Contraseña;

        _ctx.Usuario.Update(usuario);
        await _ctx.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")] // DELETE api/Usuario/{id}
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _ctx.Usuario.FindAsync(id);
        if (usuario is null) return NotFound();

        _ctx.Usuario.Remove(usuario);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    // ====== Login ======

    [HttpPost("login")] // POST api/Usuario/login
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (req is null || string.IsNullOrWhiteSpace(req.UserOrEmail) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Datos de login incompletos.");

        var normalized = req.UserOrEmail.Trim().ToUpper();

        var usuario = await _ctx.Usuario
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                (u.Email != null && u.Email.Trim().ToUpper() == normalized) ||
                (u.Nombre != null && u.Nombre.Trim().ToUpper() == normalized));

        if (usuario is null) return Unauthorized();
        if (!string.Equals(usuario.Contraseña, req.Password, StringComparison.Ordinal))
            return Unauthorized();

        var dto = new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Imagen, usuario.Rol, usuario.Contraseña);
        return Ok(dto);
    }

    // ====== Register (si tu UI usa POST api/Usuario/register) ======

    [HttpPost("register")] // POST api/Usuario/register
    public async Task<ActionResult<UsuarioDto>> Register([FromBody] UsuarioRegisterDto dto)
    {
        if (dto is null) return BadRequest();

        // Evitar duplicados por email
        if (await _ctx.Usuario.AnyAsync(u => u.Email == dto.Email))
            return Conflict($"Ya existe un usuario con email {dto.Email}");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Imagen = dto.Imagen,
            Rol = string.IsNullOrWhiteSpace(dto.Rol) ? "usuario" : dto.Rol,
            Contraseña = string.IsNullOrWhiteSpace(dto.Contrasena) ? "1234" : dto.Contrasena
        };

        _ctx.Usuario.Add(usuario);
        await _ctx.SaveChangesAsync();

        var result = new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Imagen, usuario.Rol, usuario.Contraseña);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, result);
    }
}
