using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuioscoAPI.DTOs;
using QuioscoAPI.Models;
using QuioscoAPI.Repositories.Interfaces;
using QuioscoAPI.Services.Interfaces;

namespace QuioscoAPI.Controllers;

/// <summary>Autenticación y gestión de sesión de usuarios.</summary>
[ApiController]
[Route("api")]
[Tags("Autenticación")]
public class AuthController : ControllerBase
{
    private readonly IUserRepositoryLFAH _userRepository;
    private readonly IJwtServiceLFAH _jwtService;

    public AuthController(IUserRepositoryLFAH userRepository, IJwtServiceLFAH jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    /// <summary>Iniciar sesión y obtener token JWT.</summary>
    /// <remarks>Retorna un token Bearer que debe incluirse en el header Authorization de las siguientes peticiones.</remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login([FromBody] LoginDtoLFAH dto)
    {
        var user = await _userRepository.GetByEmail(dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return UnprocessableEntity(new
            {
                errors = new { email = new[] { "Las credenciales son incorrectas." } }
            });
        }

        var token = _jwtService.GenerateToken(user);
        return Ok(new AuthResponseDtoLFAH { Token = token });
    }

    /// <summary>Registrar un nuevo usuario en el sistema.</summary>
    /// <remarks>El campo `admin` siempre es `false` al registrarse. Para asignar rol admin se debe modificar directamente en la BD.</remarks>
    [HttpPost("registro")]
    [ProducesResponseType(typeof(AuthResponseDtoLFAH), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Registro([FromBody] RegistroDtoLFAH dto)
    {
        var existingUser = await _userRepository.GetByEmail(dto.Email);
        if (existingUser is not null)
        {
            return UnprocessableEntity(new
            {
                errors = new { email = new[] { "El email ya está registrado." } }
            });
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Admin = false
        };

        var created = await _userRepository.Create(user);
        var token = _jwtService.GenerateToken(created);
        return Ok(new AuthResponseDtoLFAH { Token = token });
    }

    /// <summary>Cerrar sesión (invalida el token en el cliente).</summary>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Logout()
    {
        return Ok(new { message = "Sesión cerrada correctamente." });
    }

    /// <summary>Obtener datos del usuario autenticado.</summary>
    /// <remarks>Requiere token JWT válido. Retorna id, name, email y si es admin.</remarks>
    [Authorize]
    [HttpGet("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userRepository.GetById(userId);
        if (user is null) return NotFound();

        return Ok(new
        {
            id = user.Id,
            name = user.Name,
            email = user.Email,
            admin = user.Admin
        });
    }
}
