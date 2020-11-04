using System.Collections.Generic;

namespace GobpherLib
{
    public interface IConnection
    {
        bool Open(string domain, int port);
        List<string> Request(string path);
    }
}
