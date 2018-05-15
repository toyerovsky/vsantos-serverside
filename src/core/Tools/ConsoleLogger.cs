/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Drawing;
using Colorful;
using VRP.Core.Interfaces;

namespace VRP.Core.Tools
{
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"[Info] {message}", Color.CornflowerBlue);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"[Warning] {message}", Color.DarkGoldenrod);
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[Error] {message}", Color.DarkRed);
        }
    }
}