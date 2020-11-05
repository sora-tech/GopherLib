using GobpherLib.Facade;
using System;
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

            // Will fail on text larger than 1024
            byte[] data = new byte[1024];
            stream.Read(data, 0, 1024);

            var result = new string(Encoding.ASCII.GetChars(data));
            
            return result;
        }

        public byte[] RequestBytes(string path)
        {
            if (open == false)
            {
                throw new Exception();
            }

            var stream = client.GetStream();

            var pathBytes = Encoding.ASCII.GetBytes(path);

            stream.Write(pathBytes, 0, pathBytes.Length);
            stream.Flush();

            // Will fail on files not exactly 1024 bytes
            byte[] data = new byte[1024];
            stream.Read(data, 0, 1024);

            return data;
        }
    }
}
