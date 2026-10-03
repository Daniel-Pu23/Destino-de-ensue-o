using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public class EventService : IEventService {
    private readonly ApplicationDbContext _context;
    public EventService(ApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<EventDto>> GetUpcomingEventsAsync() {
        var events = await _context.Events.Include(e => e.Business)
            .Where(e => e.IsPublished && e.Date >= DateTime.UtcNow.Date)
            .OrderBy(e => e.Date).ToListAsync();
            
        return events.Select(e => new EventDto {
            Id = e.Id, Name = e.Name, Description = e.Description, Date = e.Date, Time = e.Time,
            Price = e.Price, IsFree = e.IsFree, Location = e.Location, BusinessId = e.BusinessId,
            BusinessName = e.Business?.Name
        });
    }
}
