using GopherLib;
using GopherLib.Connection;
using GopherLib.Facade;

namespace Gopher.Cli
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IConnection CreateAsync()
        {
            return new Async(new TcpConnection());
        }

        public IConnection CreateSimple()
        {
            return new Simple(new TcpConnection());
        }
    }
}
