using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VRP.Core.Interfaces;
using VRP.Core.Services.EventArgs;
using VRP.Core.Services.Model;

namespace VRP.Core.Services
{
    public class SocketWatcher : IWatcher
    {
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private readonly ILogger _logger;

        private Task _watcherTask;
        private readonly Socket _socketListener;

        public SocketWatcher(IPAddress ip, int port, ILogger logger)
        {
            _logger = logger;
            _socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(ip, port);
            _socketListener.Bind(endPoint);
        }

        public void StartWatching()
        {
            _watcherTask = Task.Run(() =>
            {
                try
                {
                    _socketListener.Listen(100);

                    while (true)
                    {
                        _manualResetEvent.Reset();
                        _socketListener.BeginAccept(AcceptCallback, _socketListener);
                        _manualResetEvent.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            _manualResetEvent.Set();
            Socket listener = (Socket)asyncResult.AsyncState;
            State state = new State { WorkSocket = listener.EndAccept(asyncResult) };
            //Calling Read
            state.WorkSocket.BeginReceive(state.Buffer, 0, 1024, 0, ReadCallback, state);
        }

        public event EventHandler<DataRecievedEventArgs> DataRecieved;

        private void ReadCallback(IAsyncResult asyncResult)
        {
            State state = (State)asyncResult.AsyncState;

            int bytesRead = state.WorkSocket.EndReceive(asyncResult);

            if (bytesRead > 0)
            {
                state.RecievedData.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                string content = state.RecievedData.ToString();

                DataRecievedEventArgs e = JsonConvert.DeserializeObject<DataRecievedEventArgs>(content);
                DataRecieved?.Invoke(this, e);
            }
        }

        public void Dispose()
        {
            _manualResetEvent?.Dispose();
            _watcherTask?.Dispose();
        }
    }
}