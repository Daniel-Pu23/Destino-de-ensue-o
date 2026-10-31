using DestinoEnsueno.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public interface IDiscountService
{
    Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync();
    Task<IEnumerable<DiscountDto>> GetAllForAdminAsync();
    Task<DiscountDto?> GetByIdAsync(int id);
    Task<DiscountDto> CreateAsync(DiscountCreateDto dto);
    Task<DiscountDto?> UpdateAsync(int id, DiscountCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
