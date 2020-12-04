using GopherLib.Facade;
using System;
using System.Collections.Generic;
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

            using var stream = client.GetStream();

            // Send the request selector
            var pathBytes = Encoding.ASCII.GetBytes(path);

            stream.Write(pathBytes, 0, pathBytes.Length);
            stream.Flush();

            int chunk = 1024;
            var buffer = new List<byte[]>();
            int read;
            var terminator = new byte[3] { (int)'\r', (int)'\n', (int)'.' };

            do
            {
                var data = new byte[chunk];
                try
                {
                    read = stream.Read(data, 0, chunk);
                }
                catch   //Server closed stream etc.
                {
                    break;
                }

                if (read > 0)
                {
                    buffer.Add(data[0..read]);
                }

                // read enough to contain terminator sequence
                if (read > 3 && Enumerable.SequenceEqual(data[(read - 3)..read], terminator))
                {
                    // fails if the terminator was sent over more than one chunk
                    break;
                }
            } while (read != 0);

            if (buffer.Count == 0)
            {
                return new Span<byte>();
            }

            var response = new byte[buffer.Sum(s => s.Length)];

            var position = 0;
            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i].CopyTo(response, position);
                position += buffer[i].Length;
            }

            return response;
        }

        public Task<Memory<byte>> RequestBytesAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
