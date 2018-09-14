/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Diagnostics;
using VRP.BLL.Interfaces;

namespace VRP.BLL.Tools
{
    public class DebugLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Debug.WriteLine($"[Info] {message}");
        }

        public void LogWarning(string message)
        {
            Debug.WriteLine($"[Warning] {message}");
        }

        public void LogError(string message)
        {
            Debug.WriteLine($"[Error] {message}");
        }
    }
}