using Gopher.Cli.Facade;
using GopherLib;
using System;

namespace Gopher.Cli.Display
{
    public interface IDisplay
    {
        void Draw(IConsole console);
        bool ReadKey(ConsoleKeyInfo key);

        bool CanSelect();
        Response Selected();
    }
}
