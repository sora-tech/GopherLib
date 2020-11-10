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
    public class RequestBytes
    {
        ITcpConnection connectionClient;

        [SetUp]
        public void Setup()
        {
            connectionClient = Substitute.For<ITcpConnection>();
        }

        [Test]
        public void RequestBytes_WithoutOpen_Throws()
        {
            var connection = new Async(connectionClient);

            Assert.ThrowsAsync<Exception>(async() => await connection.RequestBytesAsync(""));
        }

        [Test]
        public async Task RequestBytes_Selector_SendsSelectorValue()
        {
            var connection = new Async(connectionClient);
            var stream = new MemoryStream();
            
            connectionClient.Connected.Returns(true);
            connectionClient.GetStream().Returns<Stream>(stream);

            await connection.OpenAsync("", 0);
            await connection.RequestBytesAsync("info");

            connectionClient.Received(1).GetStream();

            byte[] data = new byte[stream.Length];

            stream.Position = 0;
            stream.Read(data, 0, data.Length);

            var result = new string(Encoding.ASCII.GetChars(data));

            Assert.AreEqual("info", result);
        }

        [Test]
        public async Task RequestBytes_SmallResponse_OnlyResponse()
        {
            var testClient = new TestClient();

            var connection = new Async(testClient);

            await connection.OpenAsync("", 0);
            var result = await connection.RequestBytesAsync("info");

            Assert.AreEqual(testClient.data.Length, result.Length);
            Assert.AreEqual(testClient.data, result.ToArray());
        }

        [Test]
        public async Task RequestBytes_LargeResponse_EntireResponse()
        {
            var testClient = new TestClient();

            var connection = new Async(testClient);

            testClient.data = new byte[2048];
            testClient.data[5] = 70;
            testClient.data[2000] = 10;

            await connection.OpenAsync("", 0);
            var result = await connection.RequestBytesAsync("info");

            Assert.AreEqual(2048, result.Length);
            Assert.AreEqual(70, result.Slice(5).Span[0]);
            Assert.AreEqual(10, result.Slice(2000).Span[0]);
        }
    }

    class TestClient : ITcpConnection
    {
        public byte[] data = new byte[] { 5, 4, 3, 2, 1 };

        public bool Connected => true;

        public void Connect(string domain, int port)
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync(string domain, int port)
        {
            return Task.CompletedTask;
        }

        public Stream GetStream()
        {
            //space for the request buffer to be written
            var request = new byte[] { 0, 0, 0, 0};

            var stream = new MemoryStream(request.Length + data.Length);
            stream.Write(request);
            stream.Write(data);
            stream.Position = 0;

            return stream;
        }
    }
}
