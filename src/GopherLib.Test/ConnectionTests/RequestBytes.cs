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
    public class RequestBytes
    {
        ITcpConnection tcpClient;

        [SetUp]
        public void Setup()
        {
            tcpClient = Substitute.For<ITcpConnection>();
        }

        [Test]
        public void BinaryRequest_WithoutOpen_Throws()
        {
            var tcp = new SimpleConnection(tcpClient);

            Assert.Throws<Exception>(() => tcp.RequestBytes(""));
        }

        [Test]
        public void RequestBytes_Selector_SendsSelectorValue()
        {
            var tcp = new SimpleConnection(tcpClient);
            var stream = new MemoryStream();
            
            tcpClient.Connected.Returns(true);
            tcpClient.GetStream().Returns<Stream>(stream);

            tcp.Open("", 0);
            tcp.RequestBytes("info");

            tcpClient.Received(1).GetStream();

            byte[] data = new byte[stream.Length];

            stream.Position = 0;
            stream.Read(data, 0, data.Length);

            var result = new string(Encoding.ASCII.GetChars(data));

            Assert.AreEqual("info", result);
        }

        [Test]
        public void RequestBytes_SmallResponse_OnlyResponse()
        {
            var testClient = new TestClient();

            var tcp = new SimpleConnection(testClient);


            tcp.Open("", 0);
            var result = tcp.RequestBytes("info");

            Assert.AreEqual(testClient.data.Length, result.Length);
            Assert.AreEqual(testClient.data, result.ToArray());
        }
    }

    class TestClient : ITcpConnection
    {
        public byte[] data = new byte[] { 5, 4, 3, 2, 1 };

        public bool Connected => true;

        public void Connect(string domain, int port)
        {
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
