﻿using Gopher.Cli.Display;
using Gopher.Cli.Facade;
using GopherLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gopher.Cli
{
    public class Browser
    {
        private readonly IConnectionFactory connectionFactory;
        private IDisplay display;
        private readonly string downloadPath;

        public Browser(IConnectionFactory factory, IConfig config)
        {
            Uri = config.Homepage();
            downloadPath = config.Downloads();
            this.connectionFactory = factory;
            this.display = new Scroller(new List<Response>(), this.Width, this.displayHeight);
            this.History = new Stack<Response>();
        }

        public int Width { get; set; } = 80;
        public int Height { get; set; } = 10;
        private int displayHeight { get => this.Height - 2; }

        public Stack<Response> History { get; private set; }

        public Uri Uri { get; private set; }

        public void Request(string selector)
        {
            var request = new Response($"1{selector}\t{selector}\t{Uri.Host}\t");

            Request(request);
        }
        public void Request(Response request)
        {
            if(request.Type == ItemType.Info)
            {
                return;
            }

            if(request.Domain != Uri.Host)
            {
                if(request.Domain.StartsWith("gopher://") == false)
                {
                    Uri = new Uri("gopher://" + request.Domain);
                }
                else
                {
                    Uri = new Uri(request.Domain);
                }
            }


            var client = new Client(Uri);

            switch (request.Type)
            {
                case ItemType.File:
                    var file = client.TextFile(connectionFactory.CreateSimple(), request.Selector);
                    this.display = new Document(file, Width, displayHeight);
                    History.Push(request);
                    break;
                case ItemType.Directory:
                    var response = client.Menu(connectionFactory.CreateSimple(), request.Selector);
                    this.display = new Scroller(response, Width, displayHeight);
                    History.Push(request);
                    break;
                case ItemType.IndexSearch:
                    var search = request.Selector.Split(" ");
                    if (search.Length > 1)
                    {
                        var searchResponse = client.Search(connectionFactory.CreateSimple(), search[0], search[1]);
                        this.display = new Scroller(searchResponse, Width, displayHeight);
                    }
                    else
                    {
                        this.display = new Search(request, Width, displayHeight);
                    }
                    break;
                case ItemType.Binary:
                case ItemType.GIF:
                case ItemType.Image:
                case ItemType.DOSBinary:
                    var binary = client.Binary(connectionFactory.CreateSimple(), request.Selector);
                    var fileName = Path.Combine(downloadPath, request.Display);
                    if (request.Display.Contains("\t"))
                    {
                        fileName = Path.Combine(downloadPath, request.Display.Split("\t")[0]);
                    }
                    if (request.Display.Contains(" "))
                    {
                        fileName = Path.Combine(downloadPath, request.Display.Split(" ")[0]);
                    }
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    File.WriteAllBytes(fileName, binary.ToArray());
                    break;
                case ItemType.Telnet:
                case ItemType.Unknown:
                case ItemType.PhoneBook:
                case ItemType.Error:
                case ItemType.BinHexed:
                case ItemType.UUEncoded:
                case ItemType.RedundantServer:
                case ItemType.TN3270:
                default:
                    break;
            }
        }

        public void Draw(IConsole console)
        {
            console.Reset();

            this.display.Draw(console);

            console.WriteLine();
            console.Write($"server: {Uri}");
        }

        public bool Input(IConsole console)
        {
            var key = console.ReadKey();

            if (display.ReadKey(key))
            {
                return true;
            }

            if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.Q)
            {
                return false;
            }

            if(key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.B)
            {
                if (this.History.Count > 1)
                {
                    this.History.Pop(); //remove current
                    Request(this.History.Pop());    // Request will re-add to stack
                    return true;
                }
            }

            if(key.Key == ConsoleKey.Enter && display.CanSelect())
            {
                Request(display.Selected());
            }

            return true;
        }
    }
}
