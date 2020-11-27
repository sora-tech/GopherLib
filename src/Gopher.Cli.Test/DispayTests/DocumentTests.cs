using Gopher.Cli.Display;
using Gopher.Cli.Facade;
using NSubstitute;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test.DisplayTests
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class DocumentTests
    {
        [Test]
        public void Document_Empty_DrawsNothing()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("", 10, 10);

            document.Draw(console);

            console.DidNotReceive().WriteLine(Arg.Any<string>());
        }

        [Test]
        public void Document_WithWidth_CropsToLength()
        {
            var console = Substitute.For<IConsole>();
            var document = new Document("this line is longer than 10 characters", 10, 10);

            document.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x == "this line "));
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
            var document = new Document("one\r\ntwo\r\nthree\r\nfour\r\nfive", 20, 3);

            document.Draw(console);

            console.Received(3).WriteLine(Arg.Any<string>());
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "four"));
            console.DidNotReceive().WriteLine(Arg.Is<string>(x => x == "five"));
        }
    }
}
