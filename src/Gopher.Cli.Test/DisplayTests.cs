using GopherLib;
using NUnit.Framework;

namespace Gopher.Cli.Test
{
    [Category("Cli")]
    public class DisplayTests
    {
        [Test]
        public void Display_Empty_PrintsEmpty()
        {
            var display = new Display(null);

            var result = display.Print();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Display_InfoResponse_OnlyDisplay()
        {
            var response = new Response("iTest Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print();

            Assert.IsNotNull(result);
            Assert.AreEqual("i Test Display", result);
        }

        [Test]
        public void Display_FileResponse_DisplayHost()
        {
            var response = new Response("0Test Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print();

            Assert.IsNotNull(result);
            Assert.AreEqual("0 Test Display : Domain Info", result);
        }

        [Test]
        public void Display_DirectoryResponse_DisplayHost()
        {
            var response = new Response("1Test Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print();

            Assert.IsNotNull(result);
            Assert.AreEqual("1 Test Display : Domain Info", result);
        }

        [Test]
        [TestCase("gTest Display\tSelector Text\tDomain Info\t71", "g Test Display : Domain Info")]
        [TestCase("ITest Display\tSelector Text\tDomain Info\t71", "I Test Display : Domain Info")]
        public void Display_ImagesResponse_DisplayHost(string input, string expected)
        {
            var response = new Response(input);
            var display = new Display(response);

            var result = display.Print();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Display_SearchResponse_DisplayHost()
        {
            var response = new Response("7Test Display\tSelector Text\tDomain Info\t71");
            var display = new Display(response);

            var result = display.Print();

            Assert.IsNotNull(result);
            Assert.AreEqual("7 Test Display : Domain Info", result);
        }

        [Test]
        [TestCase("5Test Display\tSelector Text\tDomain Info\t71", "5 Test Display : Domain Info")]
        [TestCase("9Test Display\tSelector Text\tDomain Info\t71", "9 Test Display : Domain Info")]
        public void Display_BinaryResponse_DisplayHost(string input, string expected)
        {
            var response = new Response(input);
            var display = new Display(response);

            var result = display.Print();

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
    }
}
