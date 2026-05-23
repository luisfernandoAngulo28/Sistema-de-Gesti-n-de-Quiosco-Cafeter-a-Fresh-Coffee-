using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.DTOs;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserServiceLFAH _service;

    public UsersController(IUserServiceLFAH service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetById(id);
        if (user is null) return NotFound(new { message = "Usuario no encontrado" });
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDtoLFAH dto)
    {
        var created = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDtoLFAH dto)
    {
        var updated = await _service.Update(id, dto);
        if (updated is null) return NotFound(new { message = "Usuario no encontrado" });
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.Delete(id);
        if (!deleted) return NotFound(new { message = "Usuario no encontrado" });
        return NoContent();
    }
}
