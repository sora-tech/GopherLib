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
        public void Constructor_TypeCharacter_SetsType()
        {
            const string data = "0";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.File, response.Type);
        }

        [Test]
        public void Constructor_UnknownCharacter_IsUnknownType()
        {
            const string data = "]";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.Unknown, response.Type);
        }

        [Test]
        public void Constructor_TypeOnly_KeepsDefaults()
        {
            const string data = "I";

            var response = new Response(data);

            Assert.AreEqual(ResponseType.Image, response.Type);
            Assert.IsEmpty(response.Display);
            Assert.IsEmpty(response.Selector);
            Assert.IsEmpty(response.Domain);
            Assert.AreEqual(70, response.Port);
        }
    }
}
