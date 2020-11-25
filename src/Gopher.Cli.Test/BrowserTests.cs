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
        public void Browser_InputEsc_ReturnsTrue()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            console.ReadKey().Returns(new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false));

            var result = browser.Input(console);

            Assert.IsFalse(result);
        }

        [Test]
        public void Browser_Inputq_ReturnsTrue()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            console.ReadKey().Returns(new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false));

            var result = browser.Input(console);

            Assert.IsFalse(result);
        }

        [Test]
        public void Browser_InputOther_ReturnsFalse()
        {
            var connectionfactory = Substitute.For<IConnectionFactory>();
            var console = Substitute.For<IConsole>();

            var browser = new Browser(connectionfactory, "gopher://localhost");

            console.ReadKey().Returns(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false));

            var result = browser.Input(console);

            Assert.IsTrue(result);
        }
    }
}
