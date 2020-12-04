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
        private IConnectionFactory connectionfactory;
        private IConfig config;

        [SetUp]
        public void Setup()
        {
            connectionfactory = Substitute.For<IConnectionFactory>();
            config = Substitute.For<IConfig>();
        }

        [Test]
        public void Browser_Setup_Uri()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));

            var browser = new Browser(connectionfactory, config);

            Assert.IsNotNull(browser.Uri);
            Assert.AreEqual("gopher://localhost/", browser.Uri.AbsoluteUri);
        }

        [Test]
        public void Browser_Request_MakesRequest()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));
            var connection = Substitute.For<IConnection>();
            connectionfactory.CreateSimple().Returns(connection);

            var browser = new Browser(connectionfactory, config);

            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);

            browser.Request("");

            connection.Received(1).Request(Arg.Any<string>());
        }

        [Test]
        public void Browser_NoData_DrawsEmpty()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, config);

            Assert.DoesNotThrow(() => browser.Draw(console));

            console.Received(1).Write("server: gopher://localhost/");
        }

        [Test]
        public void Browser_RequestData_SetsDisplay()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));
            var connection = Substitute.For<IConnection>();
            connectionfactory.CreateSimple().Returns(connection);
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, config);

            connection.Open(Arg.Any<string>(), Arg.Any<int>()).Returns(true);
            connection.Request(Arg.Any<string>()).Returns("0Test Display\tSelector Text\tDomain Info\t71");

            browser.Request("");

            browser.Draw(console);

            console.Received(1).Write("server: gopher://localhost/");
        }

        [Test]
        public void Browser_InputEsc_NoContinue()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, config);

            console.ReadKey().Returns(new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false));

            var result = browser.Input(console);

            Assert.IsFalse(result);
        }

        [Test]
        public void Browser_Inputq_NoContinue()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, config);

            console.ReadKey().Returns(new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false));

            var result = browser.Input(console);

            Assert.IsFalse(result);
        }

        [Test]
        public void Browser_InputOther_Continue()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, config);

            console.ReadKey().Returns(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false));

            var result = browser.Input(console);

            Assert.IsTrue(result);
        }

        [Test]
        public void BrowserHistory_Constructor_IgnoresStart()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));

            var browser = new Browser(connectionfactory, config);

            Assert.IsEmpty(browser.History);
        }

        [Test]
        public void BrowserHistory_StringRquest_AddsRequest()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));

            var browser = new Browser(connectionfactory, config);
            browser.Request("/");

            Assert.AreEqual(1, browser.History.Count);
            Assert.AreEqual("localhost", browser.History.Peek().Domain);
        }

        [Test]
        public void BrowserHistory_ObjectRquest_AddsRequestUri()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));

            var browser = new Browser(connectionfactory, config);
            browser.Request(new Response("0Test Display\tSelector\texample.com\t"));

            Assert.AreEqual(1, browser.History.Count);
            Assert.AreEqual("example.com", browser.History.Peek().Domain);
        }

        [Test]
        public void BrowserHistory_MultipleRquest_AddsAllRequestUri()
        {
            config.Homepage().Returns(new Uri("gopher://localhost"));

            var browser = new Browser(connectionfactory, config);
            browser.Request(new Response("0Test Display\tFirst\texample.com\t"));
            browser.Request(new Response("0Test Display\tSecond\texample.com\t"));

            Assert.AreEqual(2, browser.History.Count);
            Assert.AreEqual("Second", browser.History.Pop().Selector);
            Assert.AreEqual("First", browser.History.Pop().Selector);
        }
    }
}

