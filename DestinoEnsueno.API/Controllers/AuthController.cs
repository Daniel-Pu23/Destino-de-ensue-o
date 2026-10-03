using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) { _authService = authService; }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto) {
        try { return Ok(await _authService.RegisterAsync(dto, "User")); }
        catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto) {
        try { return Ok(await _authService.RegisterAsync(dto, "Admin")); }
        catch (System.Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto) {
        try { return Ok(await _authService.LoginAsync(dto)); }
        catch (System.Exception ex) { return Unauthorized(new { message = ex.Message }); }
    }
}
