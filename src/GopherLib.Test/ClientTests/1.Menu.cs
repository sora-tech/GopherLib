using GopherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ClientTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Menu
    {
        IConnection connectionSuccess;
        const string terminator = "\t\r\n\0";

        [SetUp]
        public void Setup()
        {
            connectionSuccess = Substitute.For<IConnection>();
            connectionSuccess.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
        }

        [Test]
        public void Menu_Empty_OpensConnection()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();

            client.Menu(connection, selector);

            connection.Received(1).Open(Arg.Is("example.com"), Arg.Is(70));
        }

        [Test]
        public void Menu_OpenFails_DoesNotRequest()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();
            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(false);

            client.Menu(connection, selector);

            connection.Received(0).Request(Arg.Any<string>());
        }

        [Test]
        public void Menu_Empty_RequestsUNASCII()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            client.Menu(connectionSuccess, selector);

            connectionSuccess.Received(1).Request(Arg.Is(terminator));
        }

        [Test]
        public void Menu_Path_RequestsPath()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "demo";

            client.Menu(connectionSuccess, selector);

            connectionSuccess.Received(1).Request(Arg.Is("demo" + terminator));
        }

        [Test]
        public void Menu_ResponseNull_Empty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns((string)null);

            var data = client.Menu(connectionSuccess, selector);

            Assert.IsNotNull(data);
            Assert.IsEmpty(data);
        }

        [Test]
        public void Menu_ResponseEmpty_Empty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns(string.Empty);

            var data = client.Menu(connectionSuccess, selector);

            Assert.IsNotNull(data);
            Assert.IsEmpty(data);
        }

        [Test]
        public void Menu_ResponseValid_BuildsResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns( "0Test Display\tSelector Text\tDomain Info\t71" );

            var data = client.Menu(connectionSuccess, selector);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);
        }

        [Test]
        public void Menu_ResponseMultiLine_DiscardsNull()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns("0Test Display\tSelector Text\tDomain Info\t71" + Environment.NewLine +
"3Test Second\tSelector Text\tDomain Info\t70" + Environment.NewLine +
".\0\0\0\0\0\0\0\0\0");

            var data = client.Menu(connectionSuccess, selector);

            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);
        }

        [Test]
        public void Menu_ResponseMultiLine_DiscardsPeriod()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns($"0Test Display\tSelector Text\tDomain Info\t71" + Environment.NewLine +
"2Test Second\tSelector Text\tDomain Info\t70" + Environment.NewLine +
".");

            var data = client.Menu(connectionSuccess, selector);

            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);


            var response = data[1];
            Assert.IsNotNull(response);

            Assert.AreEqual(ResponseType.PhoneBook, response.Type);
        }

        [Test]
        public void Menu_ResponseData_ParsesResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns("1Test Directory\tSelector Text\tDomain Info\t71");

            var data = client.Menu(connectionSuccess, selector);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);

            var response = data[0];
            Assert.IsNotNull(response);

            Assert.AreEqual(ResponseType.Directory, response.Type);
        }
    }
}
