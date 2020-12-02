using Gopher.Cli.Display;
using Gopher.Cli.Facade;
using GopherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test.DisplayTests
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class ScrollerTests
    {
        [Test]
        public void Scroller_Default_LineZero()
        {
            var scroller = new Scroller(new List<Response>(), 0, 0);

            Assert.AreEqual(0, scroller.Line);
        }

        [Test]
        public void Scroller_DrawEmpty_Empty()
        {
            var scroller = new Scroller(new List<Response>(), 0, 0);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(0).WriteLine(Arg.Any<string>());
        }

        [Test]
        public void Scroller_DrawResponse_CropsWidth()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 10, 1);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x.Length == 10));
        }

        [Test]
        public void Scroller_DrawResponse_PadsWidth()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 1);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x.Length == 40));
        }

        [Test]
        public void Scroller_DrawResponse_FillsEmptyLines()
        {
            var scroller = new Scroller(new List<Response> { new Response("iFirst Display\tSelector Text\tDomain Info\t71"), new Response("iSecond Display\tSelector Text\tDomain Info\t71") }, 40, 5);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            // Display first 2 lines
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  First Display                         "));
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  Second Display                        "));

            // display blank lines to fill to desired length
            console.Received(3).WriteLine();

        }

        [Test]
        public void Scroller_DrawResponse_LimitsLines()
        {
            var scroller = new Scroller(new List<Response> { new Response("iFirst Display\tSelector Text\tDomain Info\t71"), new Response("iSecond Display\tSelector Text\tDomain Info\t71") }, 40, 1);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            // Display first 1 line
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  First Display                         "));
            console.Received(0).WriteLine(Arg.Is<string>(x => x == "  Second Display                        "));

            // no blanking required
            console.Received(0).WriteLine();
        }

        [Test]
        public void Scroller_DrawSingleResponse_ScrollsDisplay()
        {
            var scroller = new Scroller(new List<Response> { new Response("iFirst Display\tSelector Text\tDomain Info\t71"), new Response("iSecond Display\tSelector Text\tDomain Info\t71") }, 40, 1);
            var console = Substitute.For<IConsole>();

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.Draw(console);

            Assert.AreEqual(1, scroller.Line);

            // Display second 1 line
            console.Received(0).WriteLine(Arg.Is<string>(x => x == "  First Display                         "));
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  Second Display                        "));

            // no blanking required
            console.Received(0).WriteLine();
        }

        [Test]
        public void Scroller_DrawTwoResponse_ScrollsDisplay()
        {
            var scroller = new Scroller(new List<Response> { new Response("iFirst Display\tSelector Text\tDomain Info\t71"), new Response("iSecond Display\tSelector Text\tDomain Info\t71"), new Response("iThird Display\tSelector Text\tDomain Info\t71") }, 40, 2);
            var console = Substitute.For<IConsole>();

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.Draw(console);

            Assert.AreEqual(1, scroller.Line);

            // Display first & second line
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  First Display                         "));
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  Second Display                        "));
            // Not scrolled into view
            console.Received(0).WriteLine(Arg.Is<string>(x => x == "  Third Display                         "));

            // no blanking required
            console.Received(0).WriteLine();
        }

        [Test]
        public void Scroller_DrawThreeResponse_ScrolledTwice_ScrollsDisplay()
        {
            var scroller = new Scroller(new List<Response> { new Response("iFirst Display\tSelector Text\tDomain Info\t71"), new Response("iSecond Display\tSelector Text\tDomain Info\t71"), new Response("iThird Display\tSelector Text\tDomain Info\t71") }, 40, 2);
            var console = Substitute.For<IConsole>();

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.Draw(console);

            Assert.AreEqual(2, scroller.Line);

            // scrolled out of view
            console.Received(0).WriteLine(Arg.Is<string>(x => x == "  First Display                         "));

            // Draw second and third
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  Second Display                        "));
            console.Received(1).WriteLine(Arg.Is<string>(x => x == "  Third Display                         "));

            // no blanking required
            console.Received(0).WriteLine();
        }

        [Test]
        public void Scroller_DrawResponse_Hilights()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 1);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).SetHilight();
            console.Received(1).SetNormal();
        }

        [Test]
        public void Scroller_DrawResponse_HilightNormal()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 2);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).SetHilight();
            console.Received(2).SetNormal();
        }

        [Test]
        public void Scroller_ReadKey_IsFalse()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 3);

            var result = scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            Assert.IsFalse(result);
        }

        [Test]
        public void Scroller_ReadKey_DownIncreasesLine()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 3);
            
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            Assert.AreEqual(1, scroller.Line);
        }

        [Test]
        public void Scroller_ReadKey_DownStopsAtLength()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 3);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            Assert.AreEqual(1, scroller.Line);
        }


        [Test]
        public void Scroller_ReadKey_UpDeIncreasesLine()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 3);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            // Check it was moved from 0 for the following assert
            Assert.AreEqual(1, scroller.Line);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));

            Assert.AreEqual(0, scroller.Line);
        }

        [Test]
        public void Scroller_ReadKey_UpStopsAtZero()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40, 3);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));

            Assert.AreEqual(0, scroller.Line);
        }

        [Test]
        public void Scroller_Selected_HasResponse()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tFirst\tDomain Info\t71"), new Response("iTest Display\tSecond\tDomain Info\t71") }, 40, 3);

            var selected = scroller.Selected();

            Assert.IsNotNull(selected);
            Assert.AreEqual("First", selected.Selector);
        }

        [Test]
        public void Scroller_SelectedMoved_IsValue()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tFirst\tDomain Info\t71"), new Response("iTest Display\tSecond\tDomain Info\t71") }, 40, 3);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            var selected = scroller.Selected();

            Assert.IsNotNull(selected);
            Assert.AreEqual("Second", selected.Selector);
        }
    }
}
