using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // this method will be used to get username from token
        public static string GetUsername(this ClaimsPrincipal user) 
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
