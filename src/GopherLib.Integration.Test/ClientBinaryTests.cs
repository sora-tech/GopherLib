using GopherLib.Connection;
using GopherLib.Facade;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GopherLib.Integration.Test
{
    [TestFixture]
    [Category("Integration")]
    [ExcludeFromCodeCoverage]
    public class ClientBinaryTests
    {
        private byte[] tcpResponse;

        private void RunServer(object serverObject)
        {
            var server = serverObject as TcpListener;
            
            server.Start();

            var client = server.AcceptTcpClient();

            var stream = client.GetStream();

            // Read the path selector from the stream
            // and discard it for this test
            var read = new byte[1024];
            stream.Read(read, 0, 1);
            stream.Flush();

            stream.Write(tcpResponse);

            stream.Close();
        }

        [Test]
        public void ClientBinary_WithServer_ReturnsData()
        {
            var rand = new Random();
            const int size = 1024;
            tcpResponse = new byte[size];
            rand.NextBytes(tcpResponse);

            var port = rand.Next(1024, 2048);

            var server = new TcpListener(IPAddress.Loopback, port);
            var thread = new Thread(RunServer);
            thread.Start(server);

            var client = new Client(new Uri($"gopher://localhost:{port}"));
            var tcpClient = new Simple(new TcpConnection());

            var result = client.Binary(tcpClient, "/data");

            Assert.AreNotEqual(0, result.Length);
            Assert.AreEqual(size, result.Length);
            Assert.AreEqual(tcpResponse, result.ToArray());
        }
    }
}