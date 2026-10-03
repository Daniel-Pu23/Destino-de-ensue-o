using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Models;
using DestinoEnsueno.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ApplicationDbContext _context;

    public EventController(IEventService eventService, ApplicationDbContext context)
    {
        _eventService = eventService;
        _context = context;
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming()
    {
        return Ok(await _eventService.GetUpcomingEventsAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] EventCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var evt = new Event
        {
            Name = dto.Name,
            Description = dto.Description,
            Date = dto.Date,
            Time = TimeSpan.TryParse(dto.Time, out var t) ? t : TimeSpan.Zero,
            Location = dto.Location ?? string.Empty,
            Organizer = dto.Organizer ?? string.Empty,
            IsFree = dto.IsFree,
            Price = dto.Price,
            BusinessId = dto.BusinessId,
            IsPublished = dto.IsPublished,
            ImageUrl = dto.ImageUrl ?? string.Empty
        };
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();
        return Created(string.Empty, new { id = evt.Id, name = evt.Name });
    }
}
