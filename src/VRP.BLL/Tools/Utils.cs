﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.IO;
using System.Reflection;

namespace VRP.BLL.Tools
{
    public static class Utils
    {
        /// <summary>
        /// Metoda zwraca następny wymiar który nie jest zajmowany przez budynek lub serwer
        /// </summary>
        /// <returns></returns>
        //public static uint GetNextFreeDimension()
        //{
        //    using (RoleplayContext ctx = Singletons.RoleplayContextFactory.Create())
        //    {
        //        //Do pierwszego budynku tak trzeba zrobić
        //        if (!ctx.Buildings.Any()) return 1;
        //        uint last = ctx.Buildings.Max(x => x.InternalDimension);
        //        do
        //            ++last; while (Enum.GetValues(typeof(Dimension)).Cast<uint>().Any(x => x == last));
        //        return last;
        //    }
        //}

        private static Random _random = new Random();
        public static int RandomRange() => _random.Next();
        public static int RandomRange(int max) => _random.Next(max);
        public static int RandomRange(int min, int max) => _random.Next(min, max);

        private static string GetWorkingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static readonly string WorkingDirectory = GetWorkingDirectory();
    }
}