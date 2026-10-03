using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public class BusinessService : IBusinessService {
    private readonly ApplicationDbContext _context;
    public BusinessService(ApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<BusinessDto>> GetAllPublishedAsync(string? search, int? categoryId) {
        var query = _context.Businesses.Include(b => b.Category).Include(b => b.Reviews).Where(b => b.IsPublished);
        
        if (!string.IsNullOrEmpty(search)) query = query.Where(b => b.Name.Contains(search) || b.Description.Contains(search));
        if (categoryId.HasValue) query = query.Where(b => b.CategoryId == categoryId.Value);

        var businesses = await query.ToListAsync();
        return businesses.Select(b => new BusinessDto {
            Id = b.Id, Name = b.Name, Description = b.Description, Address = b.Address, Phone = b.Phone,
            Latitude = b.Latitude, Longitude = b.Longitude, IsPublished = b.IsPublished, CategoryName = b.Category.Name,
            ReviewCount = b.Reviews.Count, AverageRating = b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0
        });
    }

    public async Task<BusinessDto?> GetByIdAsync(int id) {
        var b = await _context.Businesses.Include(b => b.Category).Include(b => b.Reviews).FirstOrDefaultAsync(b => b.Id == id);
        if (b == null) return null;
        return new BusinessDto {
            Id = b.Id, Name = b.Name, Description = b.Description, Address = b.Address, Phone = b.Phone,
            Latitude = b.Latitude, Longitude = b.Longitude, IsPublished = b.IsPublished, CategoryName = b.Category.Name,
            ReviewCount = b.Reviews.Count, AverageRating = b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0
        };
    }

    public async Task<BusinessDto> CreateAsync(BusinessCreateDto dto) {
        var b = new Business {
            Name = dto.Name, Description = dto.Description, Address = dto.Address, Phone = dto.Phone, Email = dto.Email,
            Latitude = dto.Latitude, Longitude = dto.Longitude, IsPublished = dto.IsPublished, CategoryId = dto.CategoryId
        };
        _context.Businesses.Add(b);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(b.Id) ?? throw new System.Exception("Error creating business");
    }
}
