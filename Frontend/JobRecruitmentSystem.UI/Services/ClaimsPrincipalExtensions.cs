using System.Security.Claims;

namespace JobRecruitmentSystem.UI.Services
{
    public static class ClaimsPrincipalExtensions
    {
        public const string AccessTokenClaimType = "AccessToken";

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        }

        public static string? GetAccessToken(this ClaimsPrincipal user)
        {
            return user.FindFirst(AccessTokenClaimType)?.Value;
        }
    }
}
