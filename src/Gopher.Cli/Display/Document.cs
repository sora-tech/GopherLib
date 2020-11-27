using Gopher.Cli.Facade;
using GopherLib;
using System;

namespace Gopher.Cli.Display
{
    public class Document : IDisplay
    {
        private readonly int width;
        private readonly int height;
        private readonly string response;
        private int line = 0;

        private readonly string[] lines;

        public Document(string response, int consoleWidth, int consoleHeight)
        {
            this.width = consoleWidth;
            this.height = consoleHeight;
            this.response = response;


            lines = response.Split("\r\n");
        }

        public void Draw(IConsole console)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return;
            }

            var limit = lines.Length > (height + line) ? (height + line): lines.Length;

            var start = line > lines.Length - height ? lines.Length - height : line;
            start = start < 0 ? 0 : start;

            for (int l = start; l < limit; l++)
            {
                string line = lines[l];
                if (line.Length > width)
                {
                    // should wrap and not trim!
                    console.WriteLine(line.Substring(0, width));
                }
                else
                {
                    var padding = new string(' ', width - line.Length);
                    console.WriteLine(line + padding);
                }
            }
        }

        public bool CanSelect() => false;
        public Response Selected() => null;

        public void ReadKey(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.DownArrow)
            {
                line = (line >= lines.Length - height) ? lines.Length - height: line + 1;
            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                line = (line > 0) ? line - 1 : 0; 
            }
        }
    }
}
