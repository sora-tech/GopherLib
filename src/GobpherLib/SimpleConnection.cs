using GobpherLib.Facade;
using System;
using System.Net.Sockets;
using System.Text;

namespace GobpherLib
{
    public class SimpleConnection : IConnection
    {
        private readonly ITcpConnection client;
        private bool open = false;

        public SimpleConnection(ITcpConnection client)
        {
            this.client = client;
        }

        public bool Open(string domain, int port)
        {
            try
            {
                client.Connect(domain, port);
                open = client.Connected;
            }
            catch { }   //Fail silently as no logging is implemented

            return open;
        }

        public string Request(string path)
        {
            if (open == false)
            {
                throw new Exception();
            }

            var stream = client.GetStream();

            var pathBytes = Encoding.ASCII.GetBytes(path);

            stream.Write(pathBytes, 0, pathBytes.Length);
            stream.Flush();

            byte[] data = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(data, 0, data.Length);

            var result = new string(Encoding.ASCII.GetChars(data));
            
            return result;
        }
    }
}
