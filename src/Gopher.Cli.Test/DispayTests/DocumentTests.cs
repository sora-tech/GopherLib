using Gopher.Cli.Display;
using Gopher.Cli.Facade;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test.DisplayTests
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class DocumentTests
    {
        private readonly ConsoleKeyInfo downKey = new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false);
        private readonly ConsoleKeyInfo upKey = new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false);

        [Test]
        public void Document_Empty_DrawsNothing()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("", 10, 10);

            document.Draw(console);

            console.DidNotReceive().WriteLine(Arg.Any<string>());
        }

        [Test]
        public void Document_WithWidth_WrapsToLength()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("this line is longer than 10 characters", 10, 10);

            document.Draw(console);

            console.Received().WriteLine(Arg.Is<string>(x => x == "this line "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "is longer "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "than 10 ch"));
            console.Received().WriteLine(Arg.Is<string>(x => x == "aracters"));
        }

        [Test]
        public void Document_WithWidth_PadsToLength()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("this line is short", 30, 10);

            document.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x == "this line is short            "));
        }

        [Test]
        public void Document_TwoLine_DrawsTwice()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("this line is longer\r\nthan one line", 20, 10);

            document.Draw(console);

            console.Received(2).WriteLine(Arg.Any<string>());
        }

        [Test]
        public void Document_MultiLine_DrawsHeight()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("one\r\ntwo\r\nthree\r\nfour\r\nfive", 5, 3);

            document.Draw(console);

            console.Received(3).WriteLine(Arg.Any<string>());
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "four "));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "five "));
        }

        [Test]
        public void Document_ScrollDown_DrawsArea()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("one\r\ntwo\r\nthree\r\nfour\r\nfive", 5, 3);

            document.ReadKey(downKey);

            document.Draw(console);

            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "one  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "two  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "three"));
            console.Received().WriteLine(Arg.Is<string>(x => x == "four "));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "five "));
        }

        [Test]
        public void Document_ScrollDown_LimitsEnd()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("one\r\ntwo\r\nthree\r\nfour\r\nfive", 5, 3);

            document.ReadKey(downKey);
            document.ReadKey(downKey);
            document.ReadKey(downKey);

            Assert.DoesNotThrow(() => document.Draw(console));

            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "one  "));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "two  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "three"));
            console.Received().WriteLine(Arg.Is<string>(x => x == "four "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "five "));
        }

        [Test]
        public void Document_ScrollUp_LimitsStart()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("one\r\ntwo\r\nthree\r\nfour\r\nfive", 5, 3);

            document.ReadKey(upKey);
            document.ReadKey(upKey);
            document.ReadKey(upKey);

            Assert.DoesNotThrow(() => document.Draw(console));

            console.Received().WriteLine(Arg.Is<string>(x => x == "one  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "two  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "three"));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "four "));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "five "));
        }

        [Test]
        public void Document_ScrollDownUp_Returns()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("one\r\ntwo\r\nthree\r\nfour\r\nfive", 5, 3);

            document.ReadKey(downKey);
            document.ReadKey(downKey);
            document.ReadKey(upKey);

            Assert.DoesNotThrow(() => document.Draw(console));

            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "one  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "two  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "three"));
            console.Received().WriteLine(Arg.Is<string>(x => x == "four "));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "five "));
        }


        [Test]
        public void Document_ScrollDownAboveLimitUp_Returns()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("one\r\ntwo\r\nthree\r\nfour\r\nfive", 5, 3);

            document.ReadKey(downKey);
            document.ReadKey(downKey);
            document.ReadKey(downKey);
            document.ReadKey(downKey);
            document.ReadKey(downKey);
            document.ReadKey(upKey);

            Assert.DoesNotThrow(() => document.Draw(console));

            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "one  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "two  "));
            console.Received().WriteLine(Arg.Is<string>(x => x == "three"));
            console.Received().WriteLine(Arg.Is<string>(x => x == "four "));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "five "));
        }
    }
}
