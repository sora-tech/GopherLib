using GopherLib.Connection;
using GopherLib.Facade;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GopherLib.Test.ConnectionTests.AsyncClient
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Open
    {
        [Test]
        public void Open_Sync_Throws()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.Connected.Returns(true);

            var tcp = new Async(tcpClient);

            Assert.Throws<NotImplementedException>(() => tcp.Open("", 0));
        }


        [Test]
        public async Task OpenAsync_ConnectedFalse_IsFalse()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.Connected.Returns(false);
            
            var tcp = new Async(tcpClient);

            var result = await tcp.OpenAsync("", 0);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task OpenAsync_ConnectedTrue_IsTrue()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.Connected.Returns(true);

            var tcp = new Async(tcpClient);

            var result = await tcp.OpenAsync("", 0);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task OpenAsync_ConnectThrows_IsFalse()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.When(c => c.Connect(Arg.Any<string>(), Arg.Any<int>())).Do((x) => throw new Exception());

            var tcp = new Async(tcpClient);

            var result = await tcp.OpenAsync("", 0);

            Assert.IsFalse(result);
        }
    }
}
