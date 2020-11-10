using GopherLib;
using GopherLib.Facade;
using System;

namespace Gopher.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Gopher!";

            var uri = new Uri("gopher://sdf.org");

            var client = new Client(uri);

            var connection = new Simple(new TcpConnection());

            var response = client.Menu(connection, "");

            foreach (var item in response)
            {
                Console.WriteLine($"{(char)item.Type} {item.Display} - {item.Selector}:{item.Domain}");
            }

            Console.ReadLine();
        }
    }
}
