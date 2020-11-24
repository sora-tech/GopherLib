using GopherLib;
using GopherLib.Connection;
using GopherLib.Facade;
using System;

namespace Gopher.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Gopher!";

            Console.BackgroundColor = ConsoleColor.Black;
            Console.BufferHeight = Console.WindowHeight;
            Console.CursorVisible = false;
            var width = Console.WindowWidth - 1;


            var uri = new Uri("gopher://sdf.org");

            var client = new Client(uri);

            var connection = new Simple(new TcpConnection());

            var response = client.Menu(connection, "");

            var selector = new Selector(response, width);

            while (true)
            {
                selector.Draw(new Facade.Console());
                Console.WriteLine();
                Console.Write($"server: {uri}");

                var key = Console.ReadKey(true);
                selector.ReadKey(key);
            }
        }
    }
}
