using GobpherLib;
using NSubstitute;
using NUnit.Framework;
using System;
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
        public void List_Path_RequestsPath()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "demo";

            client.List(connectionSuccess, path);

            connectionSuccess.Received(1).Request(Arg.Is("demo"));
        }

        [Test]
        public void List_ResponseNull_Empty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns((string)null);

            var data = client.List(connectionSuccess, path);

            Assert.IsNotNull(data);
            Assert.IsEmpty(data);
        }

        [Test]
        public void List_ResponseEmpty_Empty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns(string.Empty);

            var data = client.List(connectionSuccess, path);

            Assert.IsNotNull(data);
            Assert.IsEmpty(data);
        }

        [Test]
        public void List_ResponseValid_BuildsResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns( "0Test Display\tSelector Text\tDomain Info\t71" );

            var data = client.List(connectionSuccess, path);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);
        }

        [Test]
        public void List_ResponseMultiLine_DiscardsNull()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns("0Test Display\tSelector Text\tDomain Info\t71" + Environment.NewLine +
"3Test Second\tSelector Text\tDomain Info\t70" + Environment.NewLine +
".\0\0\0\0\0\0\0\0\0");

            var data = client.List(connectionSuccess, path);

            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);
        }

        [Test]
        public void List_ResponseMultiLine_DiscardsPeriod()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns($"0Test Display\tSelector Text\tDomain Info\t71" + Environment.NewLine +
"2Test Second\tSelector Text\tDomain Info\t70" + Environment.NewLine +
".");

            var data = client.List(connectionSuccess, path);

            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);


            var response = data[1];
            Assert.IsNotNull(response);

            Assert.AreEqual(ResponseType.PhoneBook, response.Type);
        }

        [Test]
        public void List_ResponseData_ParsesResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var path = "";
            connectionSuccess.Request(Arg.Any<string>()).Returns("1Test Directory\tSelector Text\tDomain Info\t71");

            var data = client.List(connectionSuccess, path);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);

            var response = data[0];
            Assert.IsNotNull(response);

            Assert.AreEqual(ResponseType.Directory, response.Type);
        }
    }
}
