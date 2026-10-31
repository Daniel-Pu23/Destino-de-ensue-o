using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public class BusinessService : IBusinessService
{
    private readonly ApplicationDbContext _context;

    public BusinessService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BusinessDto>> GetAllPublishedAsync(string? search, int? categoryId)
    {
        var query = _context.Businesses
            .Include(b => b.Category)
            .Include(b => b.Reviews)
            .Where(b => b.IsPublished);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(b => b.Name.Contains(search) || b.Description.Contains(search));

        if (categoryId.HasValue)
            query = query.Where(b => b.CategoryId == categoryId.Value);

        var businesses = await query.ToListAsync();
        return businesses.Select(MapToDto);
    }

    public async Task<IEnumerable<BusinessDto>> GetAllForAdminAsync()
    {
        var businesses = await _context.Businesses
            .Include(b => b.Category)
            .Include(b => b.Reviews)
            .OrderByDescending(b => b.Id)
            .ToListAsync();

        return businesses.Select(MapToDto);
    }

    public async Task<BusinessDto?> GetByIdAsync(int id)
    {
        var b = await _context.Businesses
            .Include(b => b.Category)
            .Include(b => b.Reviews)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (b == null) return null;
        return MapToDto(b);
    }

    public async Task<BusinessDto> CreateAsync(BusinessCreateDto dto)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            throw new ArgumentException("La categoría seleccionada no existe.");

        var b = new Business
        {
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Address = dto.Address.Trim(),
            Phone = dto.Phone?.Trim() ?? string.Empty,
            Email = dto.Email?.Trim() ?? string.Empty,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            IsPublished = dto.IsPublished,
            CategoryId = dto.CategoryId
        };

        _context.Businesses.Add(b);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(b.Id) ?? throw new Exception("Error al crear el negocio.");
    }

    public async Task<BusinessDto?> UpdateAsync(int id, BusinessCreateDto dto)
    {
        var b = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == id);
        if (b == null) return null;

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            throw new ArgumentException("La categoría seleccionada no existe.");

        b.Name = dto.Name.Trim();
        b.Description = dto.Description.Trim();
        b.Address = dto.Address.Trim();
        b.Phone = dto.Phone?.Trim() ?? string.Empty;
        b.Email = dto.Email?.Trim() ?? string.Empty;
        b.Latitude = dto.Latitude;
        b.Longitude = dto.Longitude;
        b.IsPublished = dto.IsPublished;
        b.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(b.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var b = await _context.Businesses
            .Include(x => x.Events)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (b == null) return false;

        foreach (var evt in b.Events)
        {
            evt.BusinessId = null;
        }

        _context.Businesses.Remove(b);
        await _context.SaveChangesAsync();
        return true;
    }

    private static BusinessDto MapToDto(Business b) => new BusinessDto
    {
        Id = b.Id,
        Name = b.Name,
        Description = b.Description,
        Address = b.Address,
        Phone = b.Phone,
        Email = b.Email,
        CategoryId = b.CategoryId,
        CategoryName = b.Category?.Name ?? string.Empty,
        Latitude = b.Latitude,
        Longitude = b.Longitude,
        IsPublished = b.IsPublished,
        ReviewCount = b.Reviews.Count,
        AverageRating = b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0
    };
}
