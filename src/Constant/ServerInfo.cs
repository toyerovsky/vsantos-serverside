/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Serverside.Core.Interfaces;

namespace Serverside.Constant
{
    public static class ServerInfo
    {
        private static string GetWorkingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static readonly string WorkingDirectory = GetWorkingDirectory();

        public static string XmlDirectory => Path.Combine(WorkingDirectory, "Xml");

        public static string JsonDirectory => Path.Combine(WorkingDirectory, "Json");

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(WorkingDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        public static IEnumerable<string> EntityDirectories
        {
            get
            {
                var directories = new List<string>();
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IXmlObject))))
                {
                    directories.Add(Path.Combine(XmlDirectory, $"{type.Name.Replace("Entity", "")}s\\"));
                }
                return directories;
            }
        }
    }
}
