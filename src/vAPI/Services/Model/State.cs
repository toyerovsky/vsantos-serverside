using System.Net.Sockets;
using System.Text;

namespace VRP.vAPI.Services.Model
{
    public class State
    {
        public Socket WorkSocket = null;
        public byte[] Buffer = new byte[1024];
        public StringBuilder RecievedData = new StringBuilder();
    }
}