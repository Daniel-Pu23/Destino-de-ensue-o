using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase {
    private readonly IBusinessService _businessService;
    public BusinessController(IBusinessService businessService) { _businessService = businessService; }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? categoryId) {
        return Ok(await _businessService.GetAllPublishedAsync(search, categoryId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) {
        var b = await _businessService.GetByIdAsync(id);
        if (b == null) return NotFound();
        return Ok(b);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BusinessCreateDto dto) {
        var b = await _businessService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = b.Id }, b);
    }
}
