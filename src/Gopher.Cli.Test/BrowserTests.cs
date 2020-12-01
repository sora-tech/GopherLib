using Gopher.Cli.Facade;
using GopherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class BrowserTests
    {
        [Test]
        public void Browser_Setup_Uri()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var browser = new Browser(connectionfactory, "gopher://localhost");

            Assert.IsNotNull(browser.Uri);
            Assert.AreEqual("gopher://localhost/", browser.Uri.AbsoluteUri);
        }

        [Test]
        public void Browser_Request_MakesRequest()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var connection = Substitute.For<IConnection>();
            connectionfactory.CreateSimple().Returns(connection);

            var browser = new Browser(connectionfactory, "gopher://localhost");

            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);

            browser.Request("");

            connection.Received(1).Request(Arg.Any<string>());
        }

        [Test]
        public void Browser_NoData_DrawsEmpty()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            Assert.DoesNotThrow(() => browser.Draw(console));

            console.Received(1).Write("server: gopher://localhost/");
        }

        [Test]
        public void Browser_RequestData_SetsDisplay()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var connection = Substitute.For<IConnection>();
            connectionfactory.CreateSimple().Returns(connection);
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
            connection.Request(Arg.Any<string>()).Returns("0Test Display\tSelector Text\tDomain Info\t71");

            browser.Request("");

            browser.Draw(console);

            console.Received(1).Write("server: gopher://localhost/");
        }

        [Test]
        public void Browser_InputEsc_NoContinue()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            console.ReadKey().Returns(new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false));

            var result = browser.Input(console);

            Assert.IsFalse(result);
        }

        [Test]
        public void Browser_Inputq_NoContinue()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            console.ReadKey().Returns(new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false));

            var result = browser.Input(console);

            Assert.IsFalse(result);
        }

        [Test]
        public void Browser_InputOther_Continue()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            console.ReadKey().Returns(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false));

            var result = browser.Input(console);

            Assert.IsTrue(result);
        }

        [Test]
        public void BrowserHistory_Constructor_IgnoresStart()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            Assert.IsEmpty(browser.History);
        }

        [Test]
        public void BrowserHistory_StringRquest_AddsRequest()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();

            var browser = new Browser(connectionfactory, "gopher://localhost");
            browser.Request("/");

            Assert.AreEqual(1, browser.History.Count);
            Assert.AreEqual("localhost", browser.History.Peek().Domain);
        }

        [Test]
        public void BrowserHistory_ObjectRquest_AddsRequestUri()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();

            var browser = new Browser(connectionfactory, "gopher://localhost");
            browser.Request(new Response("0Test Display\tSelector\texample.com\t"));

            Assert.AreEqual(1, browser.History.Count);
            Assert.AreEqual("example.com", browser.History.Peek().Domain);
        }

        [Test]
        public void BrowserHistory_MultipleRquest_AddsAllRequestUri()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();

            var browser = new Browser(connectionfactory, "gopher://localhost");
            browser.Request(new Response("0Test Display\tFirst\texample.com\t"));
            browser.Request(new Response("0Test Display\tSecond\texample.com\t"));

            Assert.AreEqual(2, browser.History.Count);
            Assert.AreEqual("Second", browser.History.Pop().Selector);
            Assert.AreEqual("First", browser.History.Pop().Selector);
        }
    }
}

