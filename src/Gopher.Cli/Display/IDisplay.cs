using Gopher.Cli.Facade;
using GopherLib;
using System;

namespace Gopher.Cli.Display
{
    public interface IDisplay
    {
        void Draw(IConsole console);
        void ReadKey(ConsoleKeyInfo key);

        bool CanSelect();
        Response Selected();
    }
}
