using Gopher.Cli.Display;
using Gopher.Cli.Facade;
using GopherLib;
using System;
using System.Collections.Generic;

namespace Gopher.Cli
{
    public class Browser
    {
        private readonly IConnectionFactory connectionFactory;
        private IDisplay display;

        public Browser(IConnectionFactory factory, string uri)
        {
            Uri = new Uri(uri);
            this.connectionFactory = factory;
            this.display = new Scroller(new List<Response>(), this.Width, this.Height - 2);
        }

        public int Width { get; set; } = 80;
        public int Height{ get; set; } = 10;

        public Uri Uri { get; private set; }

        public void Request(string selector)
        {
            var client = new Client(Uri);

            var response = client.Menu(connectionFactory.CreateSimple(), selector);

            this.display = new Scroller(response, Width, Height - 2);
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
                    this.display = new Document(file, Width, Height - 2);
                    break;
                case ItemType.Directory:
                    var response = client.Menu(connectionFactory.CreateSimple(), request.Selector);
                    this.display = new Scroller(response, Width, Height - 2);
                    break;
                case ItemType.Unknown:
                case ItemType.PhoneBook:
                case ItemType.Error:
                case ItemType.BinHexed:
                case ItemType.DOSBinary:
                case ItemType.UUEncoded:
                case ItemType.IndexSearch:
                case ItemType.Telnet:
                case ItemType.Binary:
                case ItemType.RedundantServer:
                case ItemType.TN3270:
                case ItemType.GIF:
                case ItemType.Image:
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

            if(key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.Q)
            {
                return false;
            }

            display.ReadKey(key);

            if(key.Key == ConsoleKey.Enter && display.CanSelect())
            {
                Request(display.Selected());
            }

            return true;
        }
    }
}
