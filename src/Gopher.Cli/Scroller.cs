using Gopher.Cli.Facade;
using GopherLib;
using System;
using System.Collections.Generic;

namespace Gopher.Cli
{
    public class Scroller
    {
        private readonly IList<Response> responses;
        private readonly int width;
        private readonly int height;

        public Scroller(IList<Response> responses, int consoleWidth, int consoleHeight)
        {
            this.responses = responses;
            this.width = consoleWidth;
            this.height = consoleHeight;
        }

        public int Line { get; private set; }
        public Response Selected => responses[Line];

        public void Draw(IConsole console)
        {
            // This is a very simple scroll down follow
            // there is no upwards delay which does not look nice

            var limit = responses.Count < height ? responses.Count : height;
            var start = Line >= height ? Line - height + 1 : 0;

            if(start > 0)
            {
                limit += Line - height + 1;
            }

            for (int r = start; r < limit; r++)
            {
                Response item = responses[r];
                var print = new Display(item);

                if (r == Line)
                {
                    console.SetHilight();
                }

                console.WriteLine(print.Print(width));
                console.SetNormal();
            }

            for(int p = 0; p < (height - responses.Count); p++)
            {
                console.WriteLine();
            }
        }

        public void ReadKey(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.DownArrow)
            {
                Line++;
                if(Line >= responses.Count)
                {
                    Line = responses.Count - 1;
                }
            }

            if (key.Key == ConsoleKey.UpArrow)
            {
                Line--;
                if (Line <= 0)
                {
                    Line = 0;
                }
            }
        }
    }
}
