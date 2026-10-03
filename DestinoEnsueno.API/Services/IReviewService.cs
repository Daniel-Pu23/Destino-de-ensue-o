using DestinoEnsueno.API.DTOs;
using System.Threading.Tasks;
namespace DestinoEnsueno.API.Services;
public interface IReviewService {
    Task<ReviewDto> AddOrUpdateReviewAsync(string userId, ReviewCreateDto dto);
}
