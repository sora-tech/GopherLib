using Gopher.Cli.Facade;
using GopherLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gopher.Cli.Display
{
    public class Document : IDisplay
    {
        private readonly int width;
        private readonly int height;
        private readonly string response;
        private int line = 0;

        private readonly List<string> lines;

        public Document(string response, int consoleWidth, int consoleHeight)
        {
            this.width = consoleWidth;
            this.height = consoleHeight;
            this.response = response;


            lines = response.Split("\r\n").ToList();

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if(lines[i].Length > width)
                {
                    var index = i;
                    while (i < lines.Count && lines[i].Length > width)
                    {
                        var original = lines[i].Substring(width);
                        lines[i] = lines[i].Substring(0, width);
                        lines.Insert(++i, original);
                    }
                    i = index;
                }
                else
                {
                    lines[i] = lines[i] + new string(' ', width - lines[i].Length);
                }
            }
        }

        public void Draw(IConsole console)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return;
            }

            var limit = lines.Count > (height + line) ? (height + line): lines.Count;
            var start = line > lines.Count - height ? lines.Count - height : line;
            start = start < 0 ? 0 : start;

            for (int l = start; l < limit; l++)
            {
                console.WriteLine(lines[l]);
            }
        }

        public bool CanSelect() => false;
        public Response Selected() => null;

        public void ReadKey(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.DownArrow)
            {
                line = (line >= lines.Count - height) ? lines.Count - height: line + 1;
            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                line = (line > 0) ? line - 1 : 0; 
            }
        }
    }
}
