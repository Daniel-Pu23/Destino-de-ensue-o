using DestinoEnsueno.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public interface IEventService
{
    Task<IEnumerable<EventDto>> GetUpcomingEventsAsync();
    Task<IEnumerable<EventDto>> GetAllForAdminAsync();
    Task<EventDto?> GetByIdAsync(int id);
    Task<EventDto> CreateAsync(EventCreateDto dto);
    Task<EventDto?> UpdateAsync(int id, EventCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
