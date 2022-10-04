using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SharedKernel.Extensions
{
    public static class IdentityExtensions
    {
        public static List<string> GetUserClaimRoles(this IIdentity identity)
        {
            var identity1 = identity as ClaimsIdentity;
            return identity1?.Claims?.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }       
        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            return identity?.FindFirst(claimType)?.Value;
        }

        public static string GetUserClaimValue(this IIdentity identity, string claimType)
        {
            var identity1 = identity as ClaimsIdentity;
            return identity1?.FindFirstValue(claimType);
        }

        public static T GetUserId<T>(this IIdentity identity)
        {
            var firstValue = identity?.GetUserClaimValue(ClaimTypes.NameIdentifier);
            return firstValue != null ? (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(firstValue) : default(T);
        }

        public static string GetUserName(this IIdentity identity)
        {
            return identity?.GetUserClaimValue(ClaimTypes.Name);
        }

        public static string GetUserFirstName(this IIdentity identity)
        {
            return identity?.GetUserClaimValue(ClaimTypes.GivenName);
        }

        public static string GetUserLastName(this IIdentity identity)
        {
            return identity?.GetUserClaimValue(ClaimTypes.Surname);
        }

        public static string GetUserDisplayName(this IIdentity identity)
        {
            return $"{identity?.GetUserClaimValue(ClaimTypes.GivenName)} {identity?.GetUserClaimValue(ClaimTypes.Surname)}";
        }
    }
}
