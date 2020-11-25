using Gopher.Cli.Facade;
using GopherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class ScrollerTests
    {
        [Test]
        public void Scroller_Default_LineZero()
        {
            var scroller = new Scroller(new List<Response>(), 0);

            Assert.AreEqual(0, scroller.Line);
        }

        [Test]
        public void Scroller_DrawEmpty_Empty()
        {
            var scroller = new Scroller(new List<Response>(), 0);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(0).WriteLine(Arg.Any<string>());
        }

        [Test]
        public void Scroller_DrawEmpty_Resets()
        {
            var scroller = new Scroller(new List<Response>(), 0);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).Reset();
        }

        [Test]
        public void Scroller_DrawResponse_CropsWidth()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 10);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x == "i Test Dis"));
        }

        [Test]
        public void Scroller_DrawResponse_PadsWidth()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x == "i Test Display                          "));
        }

        [Test]
        public void Scroller_DrawResponse_Hilights()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).SetHilight();
            console.Received(1).SetNormal();
        }

        [Test]
        public void Scroller_DrawResponse_HilightNormal()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            var console = Substitute.For<IConsole>();

            scroller.Draw(console);

            console.Received(1).SetHilight();
            console.Received(2).SetNormal();
        }

        [Test]
        public void Scroller_ReadKey_DownIncreasesLine()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            Assert.AreEqual(1, scroller.Line);
        }

        [Test]
        public void Scroller_ReadKey_DownStopsAtLength()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            Assert.AreEqual(1, scroller.Line);
        }


        [Test]
        public void Scroller_ReadKey_UpDeIncreasesLine()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            // Check it was moved from 0 for the following assert
            Assert.AreEqual(1, scroller.Line);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));

            Assert.AreEqual(0, scroller.Line);
        }

        [Test]
        public void Scroller_ReadKey_UpStopsAtZero()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));
            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));

            Assert.AreEqual(0, scroller.Line);
        }

        [Test]
        public void Scroller_Selected_HasResponse()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tFirst\tDomain Info\t71"), new Response("iTest Display\tSecond\tDomain Info\t71") }, 40);

            var selected = scroller.Selected;

            Assert.IsNotNull(selected);
            Assert.AreEqual("First", selected.Selector);
        }

        [Test]
        public void Scroller_SelectedMoved_IsValue()
        {
            var scroller = new Scroller(new List<Response> { new Response("iTest Display\tFirst\tDomain Info\t71"), new Response("iTest Display\tSecond\tDomain Info\t71") }, 40);

            scroller.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            var selected = scroller.Selected;

            Assert.IsNotNull(selected);
            Assert.AreEqual("Second", selected.Selector);
        }
    }
}
