using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Http;

public static class HttpContextAccessorExtensions
{
    public static string GetUserName(this IHttpContextAccessor accessor)
    {
        return accessor.HttpContext?.User?.Identity?.Name;
    }


    public static bool IsInRoles(this ClaimsPrincipal principal, params string[] RoleNames)
    {
        if (principal.IsInRole("Administrator")) return true;

        foreach (string RoleName in RoleNames)
        {
            if (principal.IsInRole(RoleName)) return true;
        }
        return false;
    }

    public static Guid VisitorId(this ClaimsPrincipal principal)
    {
        var result = principal?.Claims?.FirstOrDefault(x => x.Type == "Visitor")?.Value.Adapt<Guid?>();
        if (result == null) return Guid.NewGuid();
        return result.Value;
    }


    public static long? UserId(this ClaimsPrincipal principal)
    {
        return principal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.Adapt<long?>();
        //return principal?.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value.Adapt<long?>();
    }

    public static string FullName(this ClaimsPrincipal principal)
    {
        return principal?.Claims?.FirstOrDefault(x => x.Type == "FullName")?.Value;
    }

    public static string ImageUrl(this ClaimsPrincipal principal)
    {
        return principal?.Claims?.FirstOrDefault(x => x.Type == "ImageUrl")?.Value;
    }


    public static string GetRoleNames(this ClaimsPrincipal principal)
    {
        return string.Join(",", principal.Claims.Where(x => x.Type == ClaimTypes.Role).ToList());
    }




    public static bool HasRole(this ClaimsPrincipal principal, params string[] Roles)
    {
        foreach (var role in principal.Claims.Where(x => x.Type == ClaimTypes.Role))
        {
            if (Roles.Contains(role.Value)) return true;
        }
        return false;
    }

    public static bool HasPermission(this ClaimsPrincipal principal, params string[] Permissions)
    {
        if (principal.HasRole("Admin")) return true;
        foreach (var claim in principal.Claims.Where(x => x.Type == "Permission"))
        {
            if (Permissions.Contains(claim.Value)) return true;
        }
        return false;
    }
}
