using GopherLib;

namespace Gopher.Cli
{
    public interface IConnectionFactory
    {
        IConnection CreateSimple();
        IConnection CreateAsync();
    }
}
