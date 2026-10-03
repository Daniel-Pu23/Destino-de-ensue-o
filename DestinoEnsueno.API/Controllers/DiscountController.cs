using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Models;
using DestinoEnsueno.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;
    private readonly ApplicationDbContext _context;

    public DiscountController(IDiscountService discountService, ApplicationDbContext context)
    {
        _discountService = discountService;
        _context = context;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        return Ok(await _discountService.GetActiveDiscountsAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] DiscountCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var discount = new Discount
        {
            Name = dto.Name,
            Description = dto.Description,
            Percentage = dto.Percentage,
            Conditions = dto.Conditions ?? string.Empty,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            BusinessId = dto.BusinessId,
            IsPublished = dto.IsPublished,
            ImageUrl = dto.ImageUrl ?? string.Empty
        };
        _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();
        return Created(string.Empty, new { id = discount.Id, name = discount.Name });
    }
}
