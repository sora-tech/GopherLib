using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using GobpherLib;

namespace GopherLib.Test.ResponseTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Defaults
    {
        [Test]
        public void Response_Empty_IsNotNull()
        {
            string data = string.Empty;

            var response = new Response(data);

            Assert.IsNotNull(response);
        }

        [Test]
        public void Response_Empty_NoType()
        {
            string data = string.Empty;

            var response = new Response(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseType.Unknown, response.Type);
        }

        [Test]
        public void Response_Empty_DisplayEmpty()
        {
            string data = string.Empty;

            var response = new Response(data);

            Assert.IsNotNull(response);
            Assert.IsEmpty(response.Display);
        }

        [Test]
        public void Response_Empty_SelectorEmpty()
        {
            string data = string.Empty;

            var response = new Response(data);

            Assert.IsNotNull(response);
            Assert.IsEmpty(response.Selector);
        }

        [Test]
        public void Response_Empty_DomainEmpty()
        {
            string data = string.Empty;

            var response = new Response(data);

            Assert.IsNotNull(response);
            Assert.IsEmpty(response.Domain);
        }

        [Test]
        public void Response_Empty_HasPort()
        {
            string data = string.Empty;

            var response = new Response(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(70, response.Port);
        }
    }
}
