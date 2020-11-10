using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ClientTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Binary
    {
        IConnection connectionSuccess;
        private const string terminator = "\t\r\n\0";

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
            var testConnection = new TestDataConnection();
            testConnection.ShouldOpen = false;


            client.Binary(testConnection, selector);

            Assert.IsEmpty(testConnection.BytesPath);
        }


        [Test]
        public void Binary_Selector_RequestsUNASCII()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            var testConnection = new TestDataConnection();

            client.Binary(testConnection, selector);

            Assert.IsNotEmpty(testConnection.BytesPath);
            Assert.AreEqual(selector + terminator, testConnection.BytesPath[0]);
        }

        [Test]
        public void Binary_WithData_ReturnsResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            
            var testConnection = new TestDataConnection();

            var response = client.Binary(testConnection, selector);

            Assert.AreEqual(testConnection.RequestBytes("selector").ToArray(), response.ToArray());
        }
    }

    // Due to substitution frameworks unable to cope with Span<byte>
    // a "real" mock class is created with additions to allow inspection
    class TestDataConnection : IConnection
    {
        public List<string> BytesPath = new List<string>();
        public bool ShouldOpen = true;
        public bool Open(string domain, int port)
        {
            return ShouldOpen;
        }

        public string Request(string path)
        {
            return string.Empty;
        }

        public Span<byte> RequestBytes(string path)
        {
            BytesPath.Add(path);
            return new byte[2] { 1, 2 };
        }
    }
}
