using GobpherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ClientTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class List
    {
        IConnection connectionSuccess;

        [SetUp]
        public void Setup()
        {
            connectionSuccess = Substitute.For<IConnection>();
            connectionSuccess.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
        }

        [Test]
        public void List_Empty_OpensConnection()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";

            var connection = Substitute.For<IConnection>();

            client.List(connection, path);

            connection.Received(1).Open(Arg.Is("example.com"), Arg.Is(70));
        }

        [Test]
        public void List_OpenFails_DoesNotRequest()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";

            var connection = Substitute.For<IConnection>();
            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(false);

            client.List(connection, path);

            connection.Received(0).Request(Arg.Any<string>());
        }

        [Test]
        public void List_Empty_RequestsEmpty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";

            client.List(connectionSuccess, path);

            connectionSuccess.Received(1).Request(Arg.Is(""));
        }

        [Test]
        public void List_ResponseEmpty_Empty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns(new List<string>());

            var data = client.List(connectionSuccess, path);

            Assert.IsNotNull(data);
            Assert.IsEmpty(data);
        }
    }
}
