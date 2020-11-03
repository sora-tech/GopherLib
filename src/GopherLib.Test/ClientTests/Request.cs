using GobpherLib;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ClientTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Request
    {
        [Test]
        public void Client_Created_SetsDomain()
        {
            var domain = new Uri("gopher://example.com");

            var client = new Client(domain);

            Assert.AreEqual("example.com", client.Domain.Host);
        }

        [Test]
        public void Client_NonGopher_Throws()
        {
            var domain = new Uri("http://example.com");

            Assert.Throws<Exception>(() => new Client(domain));
        }
    }
}
