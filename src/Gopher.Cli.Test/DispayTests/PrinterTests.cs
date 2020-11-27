using Gopher.Cli.Display;
using GopherLib;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Gopher.Cli.Test.PrinterTests
{
    [TestFixture]
    [Category("Cli")]
    [ExcludeFromCodeCoverage]
    public class PrinterTests
    {
        [Test]
        public void Printer_PrintEmpty_PrintsEmpty()
        {
            var printer = new Printer(null);

            var result = printer.Print(0);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Printer_PrintLength_PrintsWhitespace()
        {
            var printer = new Printer(null);

            var result = printer.Print(10);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Length);
        }

        [Test]
        public void Printer_InfoResponse_OnlyPrinter()
        {
            var response = new Response("iTest Printer\tSelector Text\tDomain Info\t71");
            var printer = new Printer(response);

            var result = printer.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("  Test Printer                                              ", result);
        }

        [Test]
        public void Printer_FileResponse_PrinterHost()
        {
            var response = new Response("0Test Printer\tSelector Text\tDomain Info\t71");
            var printer = new Printer(response);

            var result = printer.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("F Test Printer : Domain Info                                ", result);
        }

        [Test]
        public void Printer_DirectoryResponse_PrinterHost()
        {
            var response = new Response("1Test Printer\tSelector Text\tDomain Info\t71");
            var printer = new Printer(response);

            var result = printer.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("D Test Printer : Domain Info                                ", result);
        }

        [Test]
        [TestCase("gTest Printer\tSelector Text\tDomain Info\t71", "g Test Printer : Domain Info                                ")]
        [TestCase("ITest Printer\tSelector Text\tDomain Info\t71", "I Test Printer : Domain Info                                ")]
        public void Printer_ImagesResponse_PrinterHost(string input, string expected)
        {
            var response = new Response(input);
            var printer = new Printer(response);

            var result = printer.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Printer_SearchResponse_PrinterHost()
        {
            var response = new Response("7Test Printer\tSelector Text\tDomain Info\t71");
            var printer = new Printer(response);

            var result = printer.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual("S Test Printer : Domain Info                                ", result);
        }

        [Test]
        [TestCase("5Test Printer\tSelector Text\tDomain Info\t71", "5 Test Printer : Domain Info                                ")]
        [TestCase("9Test Printer\tSelector Text\tDomain Info\t71", "9 Test Printer : Domain Info                                ")]
        public void Printer_BinaryResponse_PrinterHost(string input, string expected)
        {
            var response = new Response(input);
            var printer = new Printer(response);

            var result = printer.Print(60);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
    }
}
