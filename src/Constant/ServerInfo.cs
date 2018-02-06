/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Serverside.Constant
{
    public static class ServerInfo
    {
        public static string WorkingDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        
        public static string XmlDirectory => Path.Combine(WorkingDirectory, "Xml");

        public static string JsonDirectory => Path.Combine(WorkingDirectory, "Json");

        public static IConfigurationRoot Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(WorkingDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
