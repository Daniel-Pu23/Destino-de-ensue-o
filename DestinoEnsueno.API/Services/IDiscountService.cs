using DestinoEnsueno.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DestinoEnsueno.API.Services;
public interface IDiscountService {
    Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync();
}
