using GopherLib.Facade;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GopherLib.Connection
{
    public class Async : IConnection
    {
        private readonly ITcpConnection connection;

        public Async(ITcpConnection connection)
        {
            this.connection = connection;
        }

        public bool Open(string domain, int port)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> OpenAsync(string domain, int port)
        {
            await connection.ConnectAsync(domain, port);

            return connection.Connected;
        }

        public string Request(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RequestAsync(string path)
        {
            var data = await RequestBytesAsync(path);

            var result = new string(Encoding.ASCII.GetChars(data.ToArray()));

            return result;
        }

        public Span<byte> RequestBytes(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<Memory<byte>> RequestBytesAsync(string path)
        {
            if (this.connection.Connected != true)
            {
                throw new Exception();
            }

            var stream = connection.GetStream();

            var pathBytes = Encoding.ASCII.GetBytes(path);
            await stream.WriteAsync(pathBytes, 0, pathBytes.Length);
            await stream.FlushAsync();

            var buffer = new byte[1024];
            var data = new byte[1024];
            var size = 0;
            int read;
            do
            {
                try
                {
                    read = await stream.ReadAsync(buffer, 0, 1024);
                    buffer.CopyTo(data, size);
                    size += read;
                }
                catch   //Server closed stream etc.
                {
                    break;
                }

                if(read > 0)
                {
                    var temp = new byte[data.Length];
                    data.CopyTo(temp, 0);

                    data = new byte[data.Length + size];
                    temp.CopyTo(data, 0);
                }

            } while (read != 0);

            return data[0..size];
        }
    }
}
