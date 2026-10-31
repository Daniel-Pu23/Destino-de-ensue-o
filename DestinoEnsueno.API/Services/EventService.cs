using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EventDto>> GetUpcomingEventsAsync()
    {
        var events = await _context.Events
            .Include(e => e.Business)
            .Where(e => e.IsPublished && e.Date >= DateTime.UtcNow.Date)
            .OrderBy(e => e.Date)
            .ToListAsync();

        return events.Select(MapToDto);
    }

    public async Task<IEnumerable<EventDto>> GetAllForAdminAsync()
    {
        var events = await _context.Events
            .Include(e => e.Business)
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return events.Select(MapToDto);
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        var evt = await _context.Events
            .Include(e => e.Business)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (evt == null) return null;
        return MapToDto(evt);
    }

    public async Task<EventDto> CreateAsync(EventCreateDto dto)
    {
        if (dto.BusinessId.HasValue)
        {
            var businessExists = await _context.Businesses.AnyAsync(b => b.Id == dto.BusinessId.Value);
            if (!businessExists)
                throw new ArgumentException("El negocio asociado no existe.");
        }

        var evt = new Event
        {
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Date = dto.Date,
            Time = TimeSpan.TryParse(dto.Time, out var t) ? t : TimeSpan.Zero,
            Location = dto.Location?.Trim() ?? string.Empty,
            Organizer = dto.Organizer?.Trim() ?? string.Empty,
            IsFree = dto.IsFree,
            Price = dto.IsFree ? 0 : dto.Price,
            BusinessId = dto.BusinessId,
            IsPublished = dto.IsPublished,
            ImageUrl = dto.ImageUrl?.Trim() ?? string.Empty
        };

        _context.Events.Add(evt);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(evt.Id) ?? throw new Exception("Error al crear el evento.");
    }

    public async Task<EventDto?> UpdateAsync(int id, EventCreateDto dto)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (evt == null) return null;

        if (dto.BusinessId.HasValue)
        {
            var businessExists = await _context.Businesses.AnyAsync(b => b.Id == dto.BusinessId.Value);
            if (!businessExists)
                throw new ArgumentException("El negocio asociado no existe.");
        }

        evt.Name = dto.Name.Trim();
        evt.Description = dto.Description.Trim();
        evt.Date = dto.Date;
        evt.Time = TimeSpan.TryParse(dto.Time, out var t) ? t : TimeSpan.Zero;
        evt.Location = dto.Location?.Trim() ?? string.Empty;
        evt.Organizer = dto.Organizer?.Trim() ?? string.Empty;
        evt.IsFree = dto.IsFree;
        evt.Price = dto.IsFree ? 0 : dto.Price;
        evt.BusinessId = dto.BusinessId;
        evt.IsPublished = dto.IsPublished;
        evt.ImageUrl = dto.ImageUrl?.Trim() ?? string.Empty;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(evt.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (evt == null) return false;

        _context.Events.Remove(evt);
        await _context.SaveChangesAsync();
        return true;
    }

    private static EventDto MapToDto(Event e) => new EventDto
    {
        Id = e.Id,
        Name = e.Name,
        Description = e.Description,
        Date = e.Date,
        Time = e.Time,
        Price = e.Price,
        IsFree = e.IsFree,
        Location = e.Location,
        Organizer = e.Organizer,
        IsPublished = e.IsPublished,
        ImageUrl = e.ImageUrl,
        BusinessId = e.BusinessId,
        BusinessName = e.Business?.Name
    };
}
