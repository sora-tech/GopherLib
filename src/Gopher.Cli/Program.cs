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

            var uri = new Uri("gopher://gopher.floodgap.com");

            var client = new Client(uri);

            var connection = new SimpleConnection(new TcpConnection());

            var response = client.Search(connection, "/v2/vs", "sdf");

            foreach (var item in response)
            {
                Console.WriteLine($"{(char)item.Type} {item.Display} - {item.Selector}:{item.Domain}");
            }

            Console.ReadLine();
        }
    }
}
