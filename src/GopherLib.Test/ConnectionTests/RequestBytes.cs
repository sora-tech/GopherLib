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
    }
}
