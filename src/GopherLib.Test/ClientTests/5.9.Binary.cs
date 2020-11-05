using GobpherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ClientTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Binary
    {
        IConnection connectionSuccess;

        [SetUp]
        public void Setup()
        {
            connectionSuccess = Substitute.For<IConnection>();
            connectionSuccess.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
        }

        [Test]
        public void Binary_Empty_DoesNotOpen()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();

            client.Binary(connection, selector);

            connection.Received(0).Open(Arg.Any<string>(), Arg.Any<int>());
        }

        [Test]
        public void Binary_Empty_ReturnsEmpty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();

            var result = client.Binary(connection, selector);

            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void Binary_OpenFails_DoesNotRequest()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();
            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(false);

            client.Binary(connection, selector);

            connection.Received(0).RequestBytes(Arg.Any<string>());
        }


        [Test]
        public void Binary_Selector_RequestsSelector()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";

            client.Binary(connectionSuccess, selector);

            connectionSuccess.Received(1).RequestBytes(Arg.Is("selector"));
        }

        [Test]
        public void Binary_WithData_ReturnsResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            var testValue = new byte[2] { 1, 2 };
            connectionSuccess.RequestBytes(Arg.Is("selector")).Returns(testValue);

            var response = client.Binary(connectionSuccess, selector);

            Assert.AreEqual(testValue, response.ToArray());
        }
    }
}
