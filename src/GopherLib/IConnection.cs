using System;

namespace GopherLib
{
    public interface IConnection
    {
        bool Open(string domain, int port);
        string Request(string path);
        Span<byte> RequestBytes(string path);
    }
}
