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


        // Test fails due to the lack of visibility to the stream
        // during the execution.
        // Abstracting the stream could provide this visibility
        // or changing the internals of Request to provide
        // an interupt point
        [Test]
        public void Request_ListInfo_ReturnsResponse()
        {
            var tcp = new SimpleConnection(tcpClient);
            var stream = new MemoryStream();

            tcpClient.Connected.Returns(true);
            tcpClient.GetStream().Returns<Stream>(stream);

            tcp.Open("", 0);

            var tcpResponse = Encoding.ASCII.GetBytes("test response");
            stream.Write(tcpResponse);
            stream.Flush();

            var response = tcp.Request("info");


            Assert.AreEqual("test response", response);
        }
    }
}
