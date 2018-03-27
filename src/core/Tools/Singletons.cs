using Microsoft.Extensions.Configuration;

namespace VRP.Core.Tools
{
    public static class Singletons
    {
        public static readonly UserBroadcaster UserBroadcaster = new UserBroadcaster();

        public static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Utils.WorkingDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
    }
}