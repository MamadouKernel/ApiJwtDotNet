using boka.Models;
using System.Security.Claims;

namespace boka
{
    public interface IJwtAuthService
    {
        User Auth(string email, string password);
        string TokenGenerer(string secret, List<Claim> claims);
    }
}
