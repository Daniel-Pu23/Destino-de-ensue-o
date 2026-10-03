using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public class DiscountService : IDiscountService {
    private readonly ApplicationDbContext _context;
    public DiscountService(ApplicationDbContext context) { _context = context; }

    public async Task<IEnumerable<DiscountDto>> GetActiveDiscountsAsync() {
        var now = DateTime.UtcNow.Date;
        var discounts = await _context.Discounts.Include(d => d.Business)
            .Where(d => d.IsPublished && d.StartDate <= now && d.EndDate >= now)
            .ToListAsync();
            
        return discounts.Select(d => new DiscountDto {
            Id = d.Id, Name = d.Name, Description = d.Description, Percentage = d.Percentage,
            StartDate = d.StartDate, EndDate = d.EndDate, BusinessId = d.BusinessId,
            BusinessName = d.Business.Name
        });
    }
}
