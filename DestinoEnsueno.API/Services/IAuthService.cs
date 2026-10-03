using DestinoEnsueno.API.DTOs;
using System.Threading.Tasks;
namespace DestinoEnsueno.API.Services;
public interface IAuthService {
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto, string role);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
