using System;
using VRP.Core.Services.EventArgs;

namespace VRP.Core.Services
{
    public interface IWatcher : IDisposable
    {
        void StartWatching();
        event EventHandler<DataRecievedEventArgs> DataRecieved;
    }
}