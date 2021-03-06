﻿using GopherLib.Connection;
using GopherLib.Facade;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ConnectionTests.SimpleClient
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
            
            var tcp = new Simple(tcpClient);

            var result = tcp.Open("", 0);

            Assert.IsFalse(result);
        }

        [Test]
        public void Open_ConnectedTrue_IsTrue()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.Connected.Returns(true);

            var tcp = new Simple(tcpClient);

            var result = tcp.Open("", 0);

            Assert.IsTrue(result);
        }

        [Test]
        public void Open_ConnectThrows_IsFalse()
        {
            var tcpClient = Substitute.For<ITcpConnection>();
            tcpClient.When(c => c.Connect(Arg.Any<string>(), Arg.Any<int>())).Do((x) => throw new Exception());

            var tcp = new Simple(tcpClient);

            var result = tcp.Open("", 0);

            Assert.IsFalse(result);
        }
    }
}
