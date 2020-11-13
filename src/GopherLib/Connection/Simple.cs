using GopherLib.Facade;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherLib.Connection
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

        public Task<bool> OpenAsync(string domain, int port)
        {
            throw new NotImplementedException();
        }

        public string Request(string path)
        {
            var data = RequestBytes(path);

            var result = new string(Encoding.ASCII.GetChars(data.ToArray()));
            
            return result;
        }

        public Task<string> RequestAsync(string path)
        {
            throw new NotImplementedException();
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

                if (read > 0)
                {
                    var temp = new byte[data.Length];
                    data.CopyTo(temp, 0);

                    data = new byte[data.Length + size];
                    temp.CopyTo(data, 0);
                }

                // read enough to contain terminator sequence
                if(read > 3)
                {
                    var terminator = new byte[3] { (int)'\r', (int)'\n', (int)'.' };
                    var end = data[(read-3)..read];
                    if(Enumerable.SequenceEqual(end, terminator))
                    {
                        break;
                    }
                }


            } while (read != 0);

            return data[0..size];
        }

        public Task<Memory<byte>> RequestBytesAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
