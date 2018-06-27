using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace VRP.vAPI.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetAccountId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Claims.Single(claim => claim.Type == "AccountId").Value);
        }

        public static int GetCharacterId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Claims.Single(claim => claim.Type == "CharacterId").Value);
        }
    }
}
