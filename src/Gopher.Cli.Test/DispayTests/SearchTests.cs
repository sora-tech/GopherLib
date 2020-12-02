using Gopher.Cli.Display;
using Gopher.Cli.Facade;
using GopherLib;
using NSubstitute;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test.DispayTests
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class SearchTests
    {
        [Test]
        public void Search_Defaults_AcceptsInput()
        {
            var request = new Response("");
            var search = new Search(request, 10, 10);

            Assert.DoesNotThrow(() => search.ReadKey(new System.ConsoleKeyInfo()));
        }

        [Test]
        public void Search_Defaults_NotSelectable()
        {
            var request = new Response("");
            var search = new Search(request, 10, 10);

            Assert.IsFalse(search.CanSelect());
        }

        [Test]
        public void Search_NoInput_DrawsPlaceholder()
        {
            var console = Substitute.For<IConsole>();
            var request = new Response("");
            var search = new Search(request, 10, 10);

            search.Draw(console);

            console.Received(1).WriteLine("search: ");
        }

        [Test]
        public void Search_WithInput_DrawsInput()
        {
            var console = Substitute.For<IConsole>();
            var request = new Response("");
            var search = new Search(request, 10, 10);

            search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));
            search.Draw(console);

            console.Received(1).WriteLine("search: a");
        }

        [Test]
        public void Search_WithInput_PadsHeight()
        {
            var console = Substitute.For<IConsole>();
            var request = new Response("");
            var search = new Search(request, 10, 10);

            search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));
            search.Draw(console);

            console.Received().WriteLine("search: a");
            console.Received(9).WriteLine();
        }

        [Test]
        public void Search_WithInput_CanSelect()
        {
            var request = new Response("");
            var search = new Search(request, 10, 10);

            search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));

            Assert.IsTrue(search.CanSelect());
        }

        [Test]
        public void Search_WithInput_ReadKeyTrue()
        {
            var request = new Response("");
            var search = new Search(request, 10, 10);

            var result = search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));

            Assert.IsTrue(result);
        }


        [Test]
        public void Search_WithEnter_ReadKeyFalse()
        {
            var request = new Response("");
            var search = new Search(request, 10, 10);

            var result = search.ReadKey(new System.ConsoleKeyInfo('0', System.ConsoleKey.Enter, false, false, false));

            Assert.IsFalse(result);
        }


        [Test]
        public void Search_WithInput_GeneratesSearchRequest()
        {
            var request = new Response("7Search\tSelector\texample.com\t70");
            var search = new Search(request, 10, 10);

            search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));

            var result = search.Selected();

            Assert.IsNotNull(result);
            Assert.AreEqual(ItemType.IndexSearch, result.Type);

            Assert.AreEqual("Selector a", result.Selector);
        }

        [Test]
        public void Search_EscInput_AbandonsRequest()
        {
            var request = new Response("7Search\tSelector\texample.com\t70");
            var search = new Search(request, 10, 10);

            search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));
            var read = search.ReadKey(new System.ConsoleKeyInfo('0', System.ConsoleKey.Escape, false, false, false));

            Assert.IsFalse(read);

            var result = search.CanSelect();
            Assert.IsFalse(result);
        }

        [Test]
        public void Search_BackspaceEmpty_DoesNotThrow()
        {
            var request = new Response("7Search\tSelector\texample.com\t70");
            var search = new Search(request, 10, 10);

            Assert.DoesNotThrow(() => search.ReadKey(new System.ConsoleKeyInfo('0', System.ConsoleKey.Backspace, false, false, false)));

            var result = search.CanSelect();
            Assert.IsFalse(result);
        }

        [Test]
        public void Search_NonPrinting_Ignores()
        {
            var request = new Response("7Search\tSelector\texample.com\t70");
            var search = new Search(request, 10, 10);

            search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));
            search.ReadKey(new System.ConsoleKeyInfo('0', System.ConsoleKey.Home, false, false, false));

            var canSelect = search.CanSelect();
            Assert.IsTrue(canSelect);

            var result = search.Selected();
            Assert.AreEqual("Selector a", result.Selector);
        }

        [Test]
        public void Search_BackspaceInput_DeletesTerm()
        {
            var request = new Response("7Search\tSelector\texample.com\t70");
            var search = new Search(request, 10, 10);

            search.ReadKey(new System.ConsoleKeyInfo('a', System.ConsoleKey.A, false, false, false));
            var read = search.ReadKey(new System.ConsoleKeyInfo('0', System.ConsoleKey.Backspace, false, false, false));

            Assert.IsTrue(read);

            var result = search.CanSelect();
            Assert.IsFalse(result);
        }

        [Test]
        public void Search_LeftArrow_TermEmpty_ReadFalse()
        {
            var request = new Response("7Search\tSelector\texample.com\t70");
            var search = new Search(request, 10, 10);

            var read = search.ReadKey(new System.ConsoleKeyInfo('0', System.ConsoleKey.LeftArrow, false, false, false));

            Assert.IsFalse(read);
        }
    }
}
