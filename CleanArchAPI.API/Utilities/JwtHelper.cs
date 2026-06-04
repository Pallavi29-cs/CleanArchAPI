using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CleanArchAPI.API.Utilities;

/// <summary>
/// Helper class for reading JWT token claims.
/// Used in controllers to get current user info from token.
/// </summary>
public static class JwtHelper
{
    /// <summary>Get user GUID from JWT token claims.</summary>
    public static string? GetUserGuid(ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.NameIdentifier);

    /// <summary>Get user email from JWT token claims.</summary>
    public static string? GetUserEmail(ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Email);

    /// <summary>Get user role from JWT token claims.</summary>
    public static string? GetUserRole(ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Role);

    /// <summary>Get full name from JWT token claims.</summary>
    public static string? GetFullName(ClaimsPrincipal user)
        => user.FindFirstValue("FullName");
}
