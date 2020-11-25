using System;

namespace Gopher.Cli.Facade
{
    public interface IConsole
    {
        void SetHilight();
        void SetNormal();
        
        void WriteLine();
        void WriteLine(string text);

        void Write(string text);

        ConsoleKeyInfo ReadKey();

        void Reset();
    }
}
