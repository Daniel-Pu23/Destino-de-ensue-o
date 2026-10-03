using DestinoEnsueno.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DestinoEnsueno.API.Services;
public interface IEventService {
    Task<IEnumerable<EventDto>> GetUpcomingEventsAsync();
}
