using GopherLib.Facade;
using System;
using System.IO;
using System.Text;

namespace GopherLib
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
            var data = RequestBytes(path);

            var result = new string(Encoding.ASCII.GetChars(data.ToArray()));
            
            return result;
        }

        public Span<byte> RequestBytes(string path)
        {
            if (open == false)
            {
                throw new Exception();
            }

            var stream = client.GetStream();

            var pathBytes = Encoding.ASCII.GetBytes(path);

            stream.Write(pathBytes, 0, pathBytes.Length);
            stream.Flush();

            var data = new byte[1024];
            var size = 0;
            var buffer = stream.Read(data, 0, data.Length);
            while (buffer != 0)
            {
                try
                {
                    size += buffer;
                    buffer = stream.Read(data, 0, data.Length);
                }
                catch (IOException)  //Server closed stream etc.
                {
                    break;
                }
            }

            return data[0..size];
        }
    }
}
