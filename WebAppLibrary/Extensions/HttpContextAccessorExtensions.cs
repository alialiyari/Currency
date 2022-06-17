using System.Linq;
using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Extensions
{
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
         
        
        public static long? GetUserAgencyId(this ClaimsPrincipal principal)
        {
            var UserAgencyId = principal.Claims?.FirstOrDefault(x => x.Type == "UserAgencyId")?.Value;
            if (string.IsNullOrEmpty(UserAgencyId)) return null;
            return UserAgencyId.Adapt<long?>();
        }

        public static long? UserId(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value.Adapt<long?>();
        }
        
        public static string FullName(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "FullName")?.Value;
        }

        public static long? GePartyId(this IHttpContextAccessor accessor)
        {
            return accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "PartyId")?.Value.Adapt<long?>();
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



        public static bool IsExpert(this ClaimsPrincipal principal)
        {
            return principal.GetRoleNames().Contains("Expert");
        }
        public static bool IsOfficer(this ClaimsPrincipal principal)
        {
            return principal.GetRoleNames().Contains("Officer");
        }
        public static bool IsManager(this ClaimsPrincipal principal)
        {
            return principal.GetRoleNames().Contains("Manager");
        }
    }
}
