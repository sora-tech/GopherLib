using System.IO;

namespace GopherLib.Facade
{
    public interface ITcpConnection
    {
        bool Connected { get; }

        void Connect(string domain, int port);

        Stream GetStream();
    }
}
