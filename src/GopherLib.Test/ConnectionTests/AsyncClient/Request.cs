using GopherLib.Connection;
using GopherLib.Facade;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GopherLib.Test.ConnectionTests.AsyncClient
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Request
    {
        ITcpConnection tcpClient;

        [SetUp]
        public void Setup()
        {
            tcpClient = Substitute.For<ITcpConnection>();
        }

        [Test]
        public void Request_WithoutOpen_Throws()
        {
            var connection = new Async(tcpClient);

            Assert.ThrowsAsync<Exception>(async() => await connection.RequestAsync(""));
        }

        [Test]
        public async Task Request_ListInfo_SendsListCommand()
        {
            var connection = new Async(tcpClient);
            var stream = new MemoryStream();
            
            tcpClient.Connected.Returns(true);
            tcpClient.GetStream().Returns<Stream>(stream);

            await connection.OpenAsync("", 0);
            await connection.RequestAsync("info");

            tcpClient.Received(1).GetStream();

            byte[] data = new byte[stream.Length];

            stream.Position = 0;
            stream.Read(data, 0, data.Length);

            var result = new string(Encoding.ASCII.GetChars(data));

            Assert.AreEqual("info", result);
        }

        [Test]
        public async Task Request_ListInfo_ReturnsResponse()
        {
            var testClient = new TestRequestClient();
            var connection = new Async(testClient);

            await connection.OpenAsync("", 0);

            var response = await connection.RequestAsync("info");

            Assert.AreEqual("test response", response);
        }
    }

    class TestRequestClient : ITcpConnection
    {
        public byte[] data = Encoding.ASCII.GetBytes("test response");

        public bool Connected => true;

        public void Connect(string domain, int port)
        {
        }

        public Task ConnectAsync(string domain, int port)
        {
            return Task.CompletedTask;
        }

        public Stream GetStream()
        {
            //space for the request buffer to be written
            var request = new byte[] { 0,0,0,0 };

            var stream = new MemoryStream(request.Length + data.Length);
            stream.Write(request);
            stream.Write(data);
            stream.Position = 0;

            return stream;
        }
    }
}
