using System;
using System.Threading.Tasks;

namespace GopherLib
{
    public interface IConnection
    {
        bool Open(string domain, int port);
        Task<bool> OpenAsync(string domain, int port);

        string Request(string path);
        Task<string> RequestAsync(string path);

        Span<byte> RequestBytes(string path);
        Task<Memory<byte>> RequestBytesAsync(string path);
    }
}
