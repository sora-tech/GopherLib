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

        public Scroller(IList<Response> responses, int consoleWidth)
        {
            this.responses = responses;
            this.width = consoleWidth;
        }

        public int Line { get; private set; }
        public Response Selected => responses[Line];

        public void Draw(IConsole console)
        {
            console.Reset();

            for (int r = 0; r < responses.Count; r++)
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
