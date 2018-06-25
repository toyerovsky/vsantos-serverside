using System.Linq;
using System.Security.Claims;

namespace VRP.vAPI.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetAccountId(this ClaimsPrincipal claims)
        {
            return int.Parse(claims.Identities.Select(claim => claim.FindFirst("AccountId").Value).FirstOrDefault());
        }

        public static int GetCharacterId(this ClaimsPrincipal claims)
        {
            return int.Parse(claims.Identities.Select(claim => claim.FindFirst("CharacterId").Value).FirstOrDefault());
        }
    }
}
