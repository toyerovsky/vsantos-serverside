using System.Linq;
using Microsoft.Extensions.Configuration;

namespace VRP.vAPI.Helpers
{
    public static class RoleHelper
    {
        public static string[] SupportRanks = Startup.Configuration.GetValue<string[]>("SupportRanks");
        public static string[] AdminRoles = Startup.Configuration.GetValue<string[]>("AdminRoles");
        public static string[] DevRoles = Startup.Configuration.GetValue<string[]>("DevRoles");
        public static string[] ManagementRoles = Startup.Configuration.GetValue<string[]>("ManagementRoles");

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
