using DestinoEnsueno.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public interface IBusinessService
{
    Task<IEnumerable<BusinessDto>> GetAllPublishedAsync(string? search, int? categoryId);
    Task<IEnumerable<BusinessDto>> GetAllForAdminAsync();
    Task<BusinessDto?> GetByIdAsync(int id);
    Task<BusinessDto> CreateAsync(BusinessCreateDto dto);
    Task<BusinessDto?> UpdateAsync(int id, BusinessCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
