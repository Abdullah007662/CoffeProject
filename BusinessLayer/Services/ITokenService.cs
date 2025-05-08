using EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, IList<string> roles);
    }

   
}