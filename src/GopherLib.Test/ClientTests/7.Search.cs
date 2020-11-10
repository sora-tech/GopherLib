using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ClientTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Search
    {
        IConnection connectionSuccess;

        [SetUp]
        public void Setup()
        {
            connectionSuccess = Substitute.For<IConnection>();
            connectionSuccess.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
        }

        [Test]
        public void Search_TermEmpty_DoesNotOpen()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "";

            var connection = Substitute.For<IConnection>();

            client.Search(connection, selector, search);

            connection.Received(0).Open(Arg.Any<string>(), Arg.Any<int>());
        }

        [Test]
        public void Search_TermPopulated_OpensConnection()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";

            var connection = Substitute.For<IConnection>();

            client.Search(connection, selector, search);

            connection.Received(1).Open(Arg.Is("example.com"), Arg.Is(70));
        }

        [Test]
        public void Search_OpenFails_DoesNotRequest()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";

            var connection = Substitute.For<IConnection>();
            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(false);

            client.Search(connection, selector, search);

            connection.Received(0).Request(Arg.Any<string>());
        }

        [Test]
        public void Search_TermPopulated_RequestsUNASCII()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";

            client.Search(connectionSuccess, selector, search);

            connectionSuccess.Received(1).Request(Arg.Is("\tterm\t\r\n\0"));
        }

        [Test]
        public void Search_ResponseNull_Empty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";
            connectionSuccess.Request(Arg.Any<string>()).Returns((string)null);

            var data = client.Search(connectionSuccess, selector, search);

            Assert.IsNotNull(data);
            Assert.IsEmpty(data);
        }

        [Test]
        public void Search_ResponseEmpty_Empty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";
            connectionSuccess.Request(Arg.Any<string>()).Returns(string.Empty);

            var data = client.Search(connectionSuccess, selector, search);

            Assert.IsNotNull(data);
            Assert.IsEmpty(data);
        }

        [Test]
        public void Search_ResponseValid_BuildsResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";
            connectionSuccess.Request(Arg.Any<string>()).Returns("0Test Display\tSelector Text\tDomain Info\t71");

            var data = client.Search(connectionSuccess, selector, search);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);
        }

        [Test]
        public void Search_ResponseMultiLine_DiscardsNull()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";
            connectionSuccess.Request(Arg.Any<string>()).Returns("0Test Display\tSelector Text\tDomain Info\t71" + Environment.NewLine +
"3Test Second\tSelector Text\tDomain Info\t70" + Environment.NewLine +
".\0\0\0\0\0\0\0\0\0");

            var data = client.Search(connectionSuccess, selector, search);

            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);
        }

        [Test]
        public void Search_ResponseMultiLine_DiscardsPeriod()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";
            connectionSuccess.Request(Arg.Any<string>()).Returns($"0Test Display\tSelector Text\tDomain Info\t71" + Environment.NewLine +
"2Test Second\tSelector Text\tDomain Info\t70" + Environment.NewLine +
".");

            var data = client.Search(connectionSuccess, selector, search);

            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);


            var response = data[1];
            Assert.IsNotNull(response);

            Assert.AreEqual(ResponseType.PhoneBook, response.Type);
        }

        [Test]
        public void Search_ResponseData_ParsesResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";
            var search = "term";
            connectionSuccess.Request(Arg.Any<string>()).Returns("1Test Directory\tSelector Text\tDomain Info\t71");

            var data = client.Search(connectionSuccess, selector, search);

            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.Count);

            var response = data[0];
            Assert.IsNotNull(response);

            Assert.AreEqual(ResponseType.Directory, response.Type);
        }
    }
}
