using Gopher.Cli.Facade;
using GopherLib;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Gopher.Cli.Test
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class SelectorTests
    {
        [Test]
        public void Selector_Default_LineZero()
        {
            var selector = new Selector(new List<Response>(), 0);

            Assert.AreEqual(0, selector.Line);
        }

        [Test]
        public void Selector_DrawEmpty_Empty()
        {
            var selector = new Selector(new List<Response>(), 0);
            var console = Substitute.For<IConsole>();

            selector.Draw(console);

            console.Received(0).WriteLine(Arg.Any<string>());
        }

        [Test]
        public void Selector_DrawEmpty_Resets()
        {
            var selector = new Selector(new List<Response>(), 0);
            var console = Substitute.For<IConsole>();

            selector.Draw(console);

            console.Received(1).Reset();
        }

        [Test]
        public void Selector_DrawResponse_CropsWidth()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 10);
            var console = Substitute.For<IConsole>();

            selector.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x == "i Test Dis"));
        }

        [Test]
        public void Selector_DrawResponse_PadsWidth()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            var console = Substitute.For<IConsole>();

            selector.Draw(console);

            console.Received(1).WriteLine(Arg.Is<string>(x => x == "i Test Display                          "));
        }

        [Test]
        public void Selector_DrawResponse_Hilights()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            var console = Substitute.For<IConsole>();

            selector.Draw(console);

            console.Received(1).SetHilight();
            console.Received(1).SetNormal();
        }

        [Test]
        public void Selector_DrawResponse_HilightNormal()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            var console = Substitute.For<IConsole>();

            selector.Draw(console);

            console.Received(1).SetHilight();
            console.Received(2).SetNormal();
        }

        [Test]
        public void Selector_ReadKey_DownIncreasesLine()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);
            
            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            Assert.AreEqual(1, selector.Line);
        }

        [Test]
        public void Selector_ReadKey_DownStopsAtLength()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);

            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            Assert.AreEqual(1, selector.Line);
        }


        [Test]
        public void Selector_ReadKey_UpDeIncreasesLine()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);

            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));

            // Check it was moved from 0 for the following assert
            Assert.AreEqual(1, selector.Line);

            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));

            Assert.AreEqual(0, selector.Line);
        }

        [Test]
        public void Selector_ReadKey_UpStopsAtZero()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tSelector Text\tDomain Info\t71"), new Response("iTest Display\tSelector Text\tDomain Info\t71") }, 40);

            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));
            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));
            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.UpArrow, false, false, false));

            Assert.AreEqual(0, selector.Line);
        }

        [Test]
        public void Selector_Selected_HasResponse()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tFirst\tDomain Info\t71"), new Response("iTest Display\tSecond\tDomain Info\t71") }, 40);

            var selected = selector.Selected;

            Assert.IsNotNull(selected);
            Assert.AreEqual("First", selected.Selector);
        }

        [Test]
        public void Selector_SelectedMoved_IsValue()
        {
            var selector = new Selector(new List<Response> { new Response("iTest Display\tFirst\tDomain Info\t71"), new Response("iTest Display\tSecond\tDomain Info\t71") }, 40);

            selector.ReadKey(new ConsoleKeyInfo('0', ConsoleKey.DownArrow, false, false, false));
            var selected = selector.Selected;

            Assert.IsNotNull(selected);
            Assert.AreEqual("Second", selected.Selector);
        }
    }
}
