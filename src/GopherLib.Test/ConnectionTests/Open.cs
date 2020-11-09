using GopherLib;
using GopherLib.Facade;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ConnectionTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Open
    {
        [Test]
        public void Open_ConnectedFalse_IsFalse()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.Connected.Returns(false);
            
            var tcp = new SimpleConnection(tcpClient);

            var result = tcp.Open("", 0);

            Assert.IsFalse(result);
        }

        [Test]
        public void Open_ConnectedTrue_IsTrue()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.Connected.Returns(true);

            var tcp = new SimpleConnection(tcpClient);

            var result = tcp.Open("", 0);

            Assert.IsTrue(result);
        }

        [Test]
        public void Open_ConnectThrows_IsFalse()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.When(c => c.Connect(Arg.Any<string>(), Arg.Any<int>())).Do((x) => throw new Exception());

            var tcp = new SimpleConnection(tcpClient);

            var result = tcp.Open("", 0);

            Assert.IsFalse(result);
        }
    }
}
