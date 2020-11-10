using GopherLib.Facade;
using System;
using System.Text;

namespace GopherLib
{
    public class Simple : IConnection
    {
        private readonly ITcpConnection client;
        private bool open = false;

        public Simple(ITcpConnection client)
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

            var buffer = new byte[1024];
            var data = new byte[1024];
            var size = 0;
            int read;
            do
            {
                try
                {
                    read = stream.Read(buffer, 0, 1024);
                    buffer.CopyTo(data, size);
                    size += read;
                }
                catch   //Server closed stream etc.
                {
                    break;
                }

                if (size >= data.Length)
                {
                    var temp = new byte[data.Length];
                    data.CopyTo(temp, 0);

                    data = new byte[data.Length + 1024];
                    temp.CopyTo(data, 0);
                }


            } while (read != 0);

            return data[0..size];
        }
    }
}
