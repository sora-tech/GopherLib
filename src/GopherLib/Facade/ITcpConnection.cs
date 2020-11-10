using System.IO;
using System.Threading.Tasks;

namespace GopherLib.Facade
{
    public interface ITcpConnection
    {
        bool Connected { get; }

        void Connect(string domain, int port);

        Task ConnectAsync(string domain, int port);

        Stream GetStream();
    }
}
