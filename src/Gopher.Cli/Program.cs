using System;

namespace Gopher.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Gopher!";

            Console.BufferHeight = Console.WindowHeight;
            Console.CursorVisible = false;
            var width = Console.WindowWidth - 1;
            var height = Console.WindowHeight;

            var factory = new ConnectionFactory();
            var console = new Facade.Console();

            var browser = new Browser(factory, "gopher://sdf.org")
            {
                Width = width,
                Height = height
            };

            browser.Request("");
            browser.Draw(console);

            while (browser.Input(console))
            {
                browser.Draw(console);
            }
        }
    }
}
