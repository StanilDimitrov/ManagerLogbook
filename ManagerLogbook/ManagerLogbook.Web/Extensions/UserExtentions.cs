using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Extensions
{
    public static class UserExtentions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentException(nameof(user));
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier);

            return userId?.Value;
        }
    }
}
