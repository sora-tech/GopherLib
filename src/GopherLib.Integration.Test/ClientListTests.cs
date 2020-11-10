using GopherLib.Facade;
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
    public class ClientListTests
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

            var tcpResponse = Encoding.ASCII.GetBytes("0Test Display\tSelector Text\tDomain Info\t71" + Environment.NewLine + ".");
            stream.Write(tcpResponse);
            stream.Flush();

            stream.Close();
        }

        [Test]
        public void ClientList_WithServer_ReturnsData()
        {
            var server = new TcpListener(IPAddress.Loopback, 70);
            var thread = new Thread(RunServer);
            thread.Start(server);

            var client = new Client(new Uri("gopher://localhost"));
            var tcpClient = new Simple(new TcpConnection());

            var result = client.Menu(tcpClient, "/");

            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}