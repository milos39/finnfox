using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace finnfox.Extensions
{
    public static class IdentityExtenstions
    {
        public static string GetUserLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserLastName");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}