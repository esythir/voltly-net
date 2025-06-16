using System.Security.Claims;

namespace Voltly.Api.Extensions;

public static class IdentityExtensions
{
    public static long GetUserId(this ClaimsPrincipal principal) =>
        long.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

    public static bool IsAdmin(this ClaimsPrincipal principal) =>
        principal.IsInRole("ADMIN");
}