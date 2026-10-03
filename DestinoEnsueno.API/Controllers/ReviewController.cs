using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase {
    private readonly IReviewService _reviewService;
    private readonly DestinoEnsueno.API.Data.ApplicationDbContext _context;

    public ReviewController(IReviewService reviewService, DestinoEnsueno.API.Data.ApplicationDbContext context) {
        _reviewService = reviewService;
        _context = context;
    }

    // GET: reseñas de un negocio (público)
    [HttpGet("{businessId}")]
    public async Task<IActionResult> GetByBusiness(int businessId) {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.BusinessId == businessId)
            .OrderByDescending(r => r.Date)
            .Select(r => new ReviewDto {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                Date = r.Date,
                UserName = r.User.FirstName + " " + r.User.LastName,
                BusinessId = r.BusinessId
            })
            .ToListAsync();
        return Ok(reviews);
    }

    // POST: crear o actualizar reseña (requiere autenticación)
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddOrUpdateReview([FromBody] ReviewCreateDto dto) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        try {
            var review = await _reviewService.AddOrUpdateReviewAsync(userId, dto);
            return Ok(review);
        } catch (System.Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
