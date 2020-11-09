using System.IO;
using System.Net.Sockets;

namespace GopherLib.Facade
{
    public class TcpConnection : ITcpConnection
    {
        private readonly TcpClient client;

        public TcpConnection()
        {
            client = new TcpClient();
        }

        public bool Connected => client.Connected;

        public void Connect(string domain, int port)
        {
            client.Connect(domain, port);
        }

        public Stream GetStream()
        {
            return client.GetStream();
        }
    }
}
