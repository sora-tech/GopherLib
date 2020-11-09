using GopherLib;
using System;

namespace Gopher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.Title = "Gopher!";

            var uri = new Uri("gopher://sdf.org");

            var client = new Client(uri);

            //var response = client.Menu();
        }
    }
}
