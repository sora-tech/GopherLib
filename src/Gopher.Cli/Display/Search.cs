using Gopher.Cli.Facade;
using GopherLib;
using System;

namespace Gopher.Cli.Display
{
    public class Search : IDisplay
    {
        private string term;
        private readonly Response searchRequest;
        private readonly int Height;

        public Search(Response request, int consoleWidth, int consoleHeight)
        {
            term = string.Empty;
            searchRequest = request;
            Height = consoleHeight;
        }

        public bool CanSelect()
        {
            return term.Length != 0;
        }

        public void Draw(IConsole console)
        {
            console.WriteLine($"search: {term}");

            for (int i = 1; i < Height; i++)
                console.WriteLine();
        }

        public bool ReadKey(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.Enter)
            {
                return false;
            }

            if (key.Key == ConsoleKey.Escape)
            {
                term = string.Empty;
                return false;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                // drop last character
                if (term.Length > 0)
                { 
                    term = term[0..^1]; 
                }
                return true;
            }

            if(key.Key == ConsoleKey.LeftArrow && term.Length == 0)
            {
                return false;
            }

            if(key.KeyChar != '0')
            {
                term += key.KeyChar;
            }

            return true;
        }

        public Response Selected()
        {
            return new Response($"7{searchRequest.Display}\t{searchRequest.Selector} {term}\t{searchRequest.Domain}\t{searchRequest.Port}");
        }
    }
}
