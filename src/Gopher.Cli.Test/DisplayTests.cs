using GopherLib;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class DisplayTests
    {
        [Test]
        public void Display_PrintEmpty_PrintsEmpty()
        {
            var display = new Display(null);

            var result = display.Print(0);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Display_PrintLength_PrintsWhitespace()
        {
            var display = new Display(null);

            var result = display.Print(10);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Length);
        }

        [Test]
        public void Display_InfoResponse_OnlyDisplay()
        {
            var response = new Response("iTest Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("  Test Display                                              ", result);
        }

        [Test]
        public void Display_FileResponse_DisplayHost()
        {
            var response = new Response("0Test Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("F Test Display : Domain Info                                ", result);
        }

        [Test]
        public void Display_DirectoryResponse_DisplayHost()
        {
            var response = new Response("1Test Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("D Test Display : Domain Info                                ", result);
        }

        [Test]
        [TestCase("gTest Display\tSelector Text\tDomain Info\t71", "g Test Display : Domain Info                                ")]
        [TestCase("ITest Display\tSelector Text\tDomain Info\t71", "I Test Display : Domain Info                                ")]
        public void Display_ImagesResponse_DisplayHost(string input, string expected)
        {
            var response = new Response(input);
            var display = new Display(response);

            var result = display.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Display_SearchResponse_DisplayHost()
        {
            var response = new Response("7Test Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("S Test Display : Domain Info                                ", result);
        }

        [Test]
        [TestCase("5Test Display\tSelector Text\tDomain Info\t71", "5 Test Display : Domain Info                                ")]
        [TestCase("9Test Display\tSelector Text\tDomain Info\t71", "9 Test Display : Domain Info                                ")]
        public void Display_BinaryResponse_DisplayHost(string input, string expected)
        {
            var response = new Response(input);
            var display = new Display(response);

            var result = display.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
    }
}
