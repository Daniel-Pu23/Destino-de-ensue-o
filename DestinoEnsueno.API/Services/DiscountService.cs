using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public class DiscountService : IDiscountService
{
    private readonly ApplicationDbContext _context;

    public DiscountService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync()
    {
        var now = DateTime.UtcNow.Date;
        var discounts = await _context.Discounts
            .Include(d => d.Business)
            .Where(d => d.IsPublished && d.StartDate <= now && d.EndDate >= now)
            .OrderBy(d => d.EndDate)
            .ToListAsync();

        return discounts.Select(MapToDto);
    }

    public async Task<IEnumerable<DiscountDto>> GetAllForAdminAsync()
    {
        var discounts = await _context.Discounts
            .Include(d => d.Business)
            .OrderByDescending(d => d.EndDate)
            .ThenByDescending(d => d.Id)
            .ToListAsync();

        return discounts.Select(MapToDto);
    }

    public async Task<DiscountDto?> GetByIdAsync(int id)
    {
        var d = await _context.Discounts
            .Include(x => x.Business)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (d == null) return null;
        return MapToDto(d);
    }

    public async Task<DiscountDto> CreateAsync(DiscountCreateDto dto)
    {
        if (dto.EndDate.Date < dto.StartDate.Date)
            throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");

        var businessExists = await _context.Businesses.AnyAsync(b => b.Id == dto.BusinessId);
        if (!businessExists)
            throw new ArgumentException("El negocio asociado no existe.");

        var discount = new Discount
        {
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Percentage = dto.Percentage,
            Conditions = dto.Conditions?.Trim() ?? string.Empty,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            BusinessId = dto.BusinessId,
            IsPublished = dto.IsPublished,
            ImageUrl = dto.ImageUrl?.Trim() ?? string.Empty
        };

        _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(discount.Id) ?? throw new Exception("Error al crear el descuento.");
    }

    public async Task<DiscountDto?> UpdateAsync(int id, DiscountCreateDto dto)
    {
        var discount = await _context.Discounts.FirstOrDefaultAsync(x => x.Id == id);
        if (discount == null) return null;

        if (dto.EndDate.Date < dto.StartDate.Date)
            throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");

        var businessExists = await _context.Businesses.AnyAsync(b => b.Id == dto.BusinessId);
        if (!businessExists)
            throw new ArgumentException("El negocio asociado no existe.");

        discount.Name = dto.Name.Trim();
        discount.Description = dto.Description.Trim();
        discount.Percentage = dto.Percentage;
        discount.Conditions = dto.Conditions?.Trim() ?? string.Empty;
        discount.StartDate = dto.StartDate;
        discount.EndDate = dto.EndDate;
        discount.BusinessId = dto.BusinessId;
        discount.IsPublished = dto.IsPublished;
        discount.ImageUrl = dto.ImageUrl?.Trim() ?? string.Empty;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(discount.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var discount = await _context.Discounts.FirstOrDefaultAsync(x => x.Id == id);
        if (discount == null) return false;

        _context.Discounts.Remove(discount);
        await _context.SaveChangesAsync();
        return true;
    }

    private static DiscountDto MapToDto(Discount d) => new DiscountDto
    {
        Id = d.Id,
        Name = d.Name,
        Description = d.Description,
        Percentage = d.Percentage,
        Conditions = d.Conditions,
        StartDate = d.StartDate,
        EndDate = d.EndDate,
        IsPublished = d.IsPublished,
        ImageUrl = d.ImageUrl,
        BusinessId = d.BusinessId,
        BusinessName = d.Business?.Name ?? string.Empty
    };
}
