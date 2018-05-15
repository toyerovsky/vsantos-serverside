using System.Diagnostics;
using VRP.Core.Interfaces;

namespace VRP.Core.Tools
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