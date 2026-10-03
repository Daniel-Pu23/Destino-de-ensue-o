using DestinoEnsueno.API.Data;
using DestinoEnsueno.API.DTOs;
using DestinoEnsueno.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DestinoEnsueno.API.Services;

public class ReviewService : IReviewService {
    private readonly ApplicationDbContext _context;
    public ReviewService(ApplicationDbContext context) { _context = context; }

    public async Task<ReviewDto> AddOrUpdateReviewAsync(string userId, ReviewCreateDto dto) {
        if (dto.Rating < 1 || dto.Rating > 5) throw new ArgumentException("Rating must be between 1 and 5");

        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.BusinessId == dto.BusinessId);
        if (review == null) {
            review = new Review { UserId = userId, BusinessId = dto.BusinessId, Rating = dto.Rating, Comment = dto.Comment, Date = DateTime.UtcNow };
            _context.Reviews.Add(review);
        } else {
            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.Date = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);
        return new ReviewDto { Id = review.Id, Rating = review.Rating, Comment = review.Comment, Date = review.Date, BusinessId = review.BusinessId, UserName = user!.FirstName };
    }
}
