using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VRP.vAPI.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetAccountId(this ClaimsPrincipal claims)
        {
            return int.Parse(claims.Identities.Select(claim => claim.FindFirst("CharacterId").Value).First());
        }
    }
}
