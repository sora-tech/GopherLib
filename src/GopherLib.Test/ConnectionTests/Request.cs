using GobpherLib;
using GobpherLib.Facade;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace GopherLib.Test.ConnectionTests
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
            var tcp = new SimpleConnection(tcpClient);

            Assert.Throws<Exception>(() => tcp.Request(""));
        }

        [Test]
        public void Request_ListInfo_SendsListCommand()
        {
            var tcp = new SimpleConnection(tcpClient);
            var stream = new MemoryStream();
            
            tcpClient.Connected.Returns(true);
            tcpClient.GetStream().Returns<Stream>(stream);

            tcp.Open("", 0);
            tcp.Request("info");

            tcpClient.Received(1).GetStream();

            byte[] data = new byte[stream.Length];

            stream.Position = 0;
            stream.Read(data, 0, data.Length);

            var result = new string(Encoding.ASCII.GetChars(data));

            Assert.AreEqual("info", result);
        }

        [Test]
        public void Request_ListInfo_ReturnsResponse()
        {
            var testClient = new TestRequestClient();
            var tcp = new SimpleConnection(testClient);

            tcp.Open("", 0);

            var response = tcp.Request("info");

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
