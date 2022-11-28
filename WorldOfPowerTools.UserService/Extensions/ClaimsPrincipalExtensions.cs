using System.Security.Claims;

namespace WorldOfPowerTools.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public const string CLAIM_USER_GUID = "guid";

        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            if (Guid.TryParse(user.FindFirstValue(CLAIM_USER_GUID), out var userUuid))
            {
                return userUuid;
            }

            return null;
        }
    }
}
