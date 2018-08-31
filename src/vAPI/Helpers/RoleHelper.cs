using System.Linq;
using Microsoft.Extensions.Configuration;

namespace VRP.vAPI.Helpers
{
    public static class RoleHelper
    {
        public static string[] SupportRanks = Startup.Configuration.GetSection("Ranks:Support").Get<string[]>();
        public static string[] AdminRoles = Startup.Configuration.GetSection("Ranks:Admin").Get<string[]>();
        public static string[] DevRoles = Startup.Configuration.GetSection("Ranks:Dev").Get<string[]>();
        public static string[] ManagementRoles = Startup.Configuration.GetSection("Ranks:Management").Get<string[]>();

        public static string[] GetFromSupportRoles()
        {
            return SupportRanks.Concat(AdminRoles).Concat(DevRoles).Concat(ManagementRoles).ToArray();
        }

        public static string[] GetFromAdminRoles()
        {
            return AdminRoles.Concat(DevRoles).Concat(ManagementRoles).ToArray();
        }

        public static string[] GetFromDevRoles()
        {
            return DevRoles.Concat(ManagementRoles).ToArray();
        }
    }
}
