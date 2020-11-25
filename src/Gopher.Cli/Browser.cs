using Gopher.Cli.Facade;
using GopherLib;
using System;
using System.Collections.Generic;

namespace Gopher.Cli
{
    public class Browser
    {
        private readonly IConnectionFactory connectionFactory;
        private Selector selector;

        public Browser(IConnectionFactory factory, string uri)
        {
            Uri = new Uri(uri);
            this.connectionFactory = factory;
            this.selector = new Selector(new List<Response>(), this.Width);
        }

        public int Width { get; set; } = 80;
        public Uri Uri { get; private set; }

        public void Request(string selector)
        {
            var client = new Client(Uri);

            var response = client.Menu(connectionFactory.CreateSimple(), selector);

            this.selector = new Selector(response, Width);
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
            var response = new List<Response>();
            switch (request.Type)
            {
                case ItemType.Unknown:
                    break;
                case ItemType.File:
                    break;
                case ItemType.Directory:
                    response = client.Menu(connectionFactory.CreateSimple(), request.Selector);
                    break;
                case ItemType.PhoneBook:
                    break;
                case ItemType.Error:
                    break;
                case ItemType.BinHexed:
                    break;
                case ItemType.DOSBinary:
                    break;
                case ItemType.UUEncoded:
                    break;
                case ItemType.IndexSearch:
                    break;
                case ItemType.Telnet:
                    break;
                case ItemType.Binary:
                    break;
                case ItemType.RedundantServer:
                    break;
                case ItemType.TN3270:
                    break;
                case ItemType.GIF:
                    break;
                case ItemType.Image:
                    break;
                default:
                    break;
            }

            this.selector = new Selector(response, Width);
        }

        public void Draw(IConsole console)
        {
            this.selector.Draw(console);
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

            selector.ReadKey(key);

            if(key.Key == ConsoleKey.Enter)
            {
                Request(selector.Selected);
            }

            return true;
        }
    }
}
