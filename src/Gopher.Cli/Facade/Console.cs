﻿using System;

namespace Gopher.Cli.Facade
{
    public class Console : IConsole
    {
        public ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey(true);
        }

        public void Reset()
        {
            System.Console.Clear();
            System.Console.SetCursorPosition(0, 0);
        }

        public void SetHilight()
        {
            System.Console.ForegroundColor = ConsoleColor.Black;
            System.Console.BackgroundColor = ConsoleColor.White;
        }

        public void SetNormal()
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.Black;
        }

        public void Write(string text)
        {
            System.Console.Write(text);
        }

        public void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }

        public void WriteLine()
        {
            System.Console.WriteLine();
        }
    }
}
