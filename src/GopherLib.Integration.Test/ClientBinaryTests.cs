using GobpherLib;
using GobpherLib.Facade;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GopherLib.Integration.Test
{
    [TestFixture]
    [Category("Integration")]
    [ExcludeFromCodeCoverage]
    public class ClientBinaryTests
    {
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

            var tcpResponse = new byte[] { 5, 4, 3, 2, 1 };
            stream.Write(tcpResponse);

            stream.Close();
        }

        [Test]
        public void ClientBinary_WithServer_ReturnsData()
        {
            var server = new TcpListener(IPAddress.Loopback, 70);
            var thread = new Thread(RunServer);
            thread.Start(server);

            var client = new Client(new Uri("gopher://localhost"));
            var tcpClient = new SimpleConnection(new TcpConnection());

            var result = client.Binary(tcpClient, "/data");

            var expected = new byte[] { 5, 4, 3, 2, 1 };

            Assert.AreNotEqual(0, result.Length);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual(expected, result.ToArray());
        }
    }
}