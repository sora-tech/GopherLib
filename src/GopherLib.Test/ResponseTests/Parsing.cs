using GobpherLib;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ResponseTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Parsing
    {
        [Test]
        public void Constructor_UnknownCharacter_IsUnknownType()
        {
            const string data = "]\t\t\t";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.Unknown, response.Type);
        }

        [Test]
        public void Constructor_TypeOnly_KeepsDefaults()
        {
            const string data = "I\t\t\t";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.Image, response.Type);
            Assert.IsEmpty(response.Display);
            Assert.IsEmpty(response.Selector);
            Assert.IsEmpty(response.Domain);
            Assert.AreEqual(70, response.Port);
        }

        [Test]
        public void Constructor_TwoTabs_IsUnknown()
        {
            const string data = "0\t\t";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.Unknown, response.Type);
        }

        [Test]
        public void Constructor_ThreeTabs_IsType()
        {
            const string data = "0\t\t\t";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.File, response.Type);
        }

        [Test]
        public void Constructor_FourTabs_Accepted()
        {
            const string data = "0\t\t\t\t";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.File, response.Type);
        }

        [Test]
        public void Constructor_TypeDisplay_SplitsDisplay()
        {
            const string data = "0Test Display\t\t\t";

            var response = new Response(data);

            Assert.AreEqual("Test Display", response.Display);
        }

        [Test]
        public void Constructor_Selector_SplitsSelector()
        {
            const string data = "0Test Display\tSelector Text\t\t";

            var response = new Response(data);

            Assert.AreEqual("Selector Text", response.Selector);
        }

        [Test]
        public void Constructor_Domain_SplitsDomain()
        {
            const string data = "0Test Display\tSelector Text\tDomain Info\t";

            var response = new Response(data);

            Assert.AreEqual("Domain Info", response.Domain);
        }

        [Test]
        public void Constructor_Port_SplitsPort()
        {
            const string data = "0Test Display\tSelector Text\tDomain Info\t71";

            var response = new Response(data);

            Assert.AreEqual(71, response.Port);
        }


        [Test]
        public void Constructor_InvalidPort_KeepsDefault()
        {
            const string data = "0Test Display\tSelector Text\tDomain Info\tAB";

            var response = new Response(data);

            Assert.AreEqual(70, response.Port);
        }
    }
}
