using System;

namespace VRP.Core.Services
{
    public interface IBroadcaster : IDisposable
    {
        void Broadcast(string json);
    }
}