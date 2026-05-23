using QuioscoAPI.Models;

namespace QuioscoAPI.Services.Interfaces;

public interface IJwtServiceLFAH
{
    string GenerateToken(User user);
}
