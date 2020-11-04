using System.IO;

namespace GobpherLib.Facade
{
    public interface ITcpConnection
    {
        bool Connected { get; }

        void Connect(string domain, int port);

        Stream GetStream();
    }
}
