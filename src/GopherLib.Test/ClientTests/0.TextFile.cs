using GopherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ClientTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TextFile
    {
        IConnection connectionSuccess;

        [SetUp]
        public void Setup()
        {
            connectionSuccess = Substitute.For<IConnection>();
            connectionSuccess.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
        }

        [Test]
        public void TextFile_Empty_DoesNotOpen()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();

            client.TextFile(connection, selector);

            connection.Received(0).Open(Arg.Any<string>(), Arg.Any<int>());
        }

        [Test]
        public void TextFile_Empty_ReturnsEmpty()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();

            var result = client.TextFile(connection, selector);

            Assert.IsEmpty(result);
        }

        [Test]
        public void TextFile_OpenFails_DoesNotRequest()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "";

            var connection = Substitute.For<IConnection>();
            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(false);

            client.TextFile(connection, selector);

            connection.Received(0).Request(Arg.Any<string>());
        }

        [Test]
        public void TextFile_Selector_RequestsSelector()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";

            client.TextFile(connectionSuccess, selector);

            connectionSuccess.Received(1).Request(Arg.Is("selector"));
        }

        [Test]
        public void TextFile_WithData_ReturnsResponse()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            const string testValue = "test data file";
            connectionSuccess.Request(Arg.Is("selector")).Returns(testValue);

            var response = client.TextFile(connectionSuccess, selector);

            Assert.AreEqual(testValue, response);
        }

        [Test]
        public void TextFile_LastLineIncluded_Trims()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            const string testValue = "test data file";
            string responseValue = testValue + "." + Environment.NewLine;   //Append the LASTLINE value ".CRLF"

            connectionSuccess.Request(Arg.Is("selector")).Returns(responseValue);

            var response = client.TextFile(connectionSuccess, selector);

            Assert.AreEqual(testValue, response);
        }

        [Test]
        public void TextFile_NullEnded_Trims()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            const string testValue = "test data file";
            string responseValue = testValue + "." + Environment.NewLine;   //Append the LASTLINE value ".CRLF"
            responseValue += "\0\0\0\0\0\0";

            connectionSuccess.Request(Arg.Is("selector")).Returns(responseValue);

            var response = client.TextFile(connectionSuccess, selector);

            Assert.AreEqual(testValue, response);
        }

        /*
         * Note:  Lines beginning with periods must be prepended with an extra
         * period to ensure that the transmission is not terminated early.
         * The client should strip extra periods at the beginning of the line.
        */
        [Test]
        public void TextFile_DoublePeriodLineStart_RemovesPeriod()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            const string testValue = @"line
..
test.
";
            const string expectedValue = @"line
.
test";

            connectionSuccess.Request(Arg.Is("selector")).Returns(testValue);

            var response = client.TextFile(connectionSuccess, selector);

            Assert.AreEqual(expectedValue, response);
        }

        [Test]
        public void TextFile_DoublePeriodWithinLine_Leaves()
        {
            var client = new Client(new Uri("gopher://example.com"));
            var selector = "selector";
            const string testValue = @"line..
..
test.
";
            const string expectedValue = @"line..
.
test";

            connectionSuccess.Request(Arg.Is("selector")).Returns(testValue);

            var response = client.TextFile(connectionSuccess, selector);

            Assert.AreEqual(expectedValue, response);
        }
    }
}
