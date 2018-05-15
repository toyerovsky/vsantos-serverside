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