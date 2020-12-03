using System;

namespace Gopher.Cli
{
    public interface IConfig
    {
        Uri Homepage();

        string Downloads();
    }
}
