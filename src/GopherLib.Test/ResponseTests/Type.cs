using GobpherLib;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GopherLib.Test.ResponseTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Type
    {
        [Test]
        public void ResponseType_Unknown_NotKnown()
        {
            //Given the list of supported types from the RFC
            //the value of Uknown must not be part of them

            var knownTypes = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', 'T', 'g', 'I' };

            var unknown = ResponseType.Unknown;

            CollectionAssert.DoesNotContain(knownTypes, unknown);
        }

        [Test]
        public void ResponseType_Unknown_NotReserved()
        {
            //Given the list of reserved types from the RFC
            //the value of Uknown must not be part of them

            var knownTypes = new List<char>();
            
            // Upper case letters
            for(int c = 65; c < 91; c++)
            {
                knownTypes.Add((char)c);
                knownTypes.Add((char)(c + 32)); // Shift to lower case
            }

            var unknown = ResponseType.Unknown;

            CollectionAssert.DoesNotContain(knownTypes, unknown);
        }

        [Test]
        public void ResponseType_Zero_IsFile()
        {
            const char responseCode = '0';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.File, responseType);
        }

        [Test]
        public void ResponseType_One_IsDirectory()
        {
            const char responseCode = '1';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.Directory, responseType);
        }

        [Test]
        public void ResponseType_Two_IsPhoneBook()
        {
            const char responseCode = '2';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.PhoneBook, responseType);
        }

        [Test]
        public void ResponseType_Three_IsError()
        {
            const char responseCode = '3';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.Error, responseType);
        }

        [Test]
        public void ResponseType_Four_IsBinHexed()
        {
            const char responseCode = '4';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.BinHexed, responseType);
        }

        [Test]
        public void ResponseType_Five_IsDOSBinary()
        {
            const char responseCode = '5';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.DOSBinary, responseType);
        }

        [Test]
        public void ResponseType_Six_IsUUEncoded()
        {
            const char responseCode = '6';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.UUEncoded, responseType);
        }

        [Test]
        public void ResponseType_Seven_IsIndexSearch()
        {
            const char responseCode = '7';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.IndexSearch, responseType);
        }

        [Test]
        public void ResponseType_Eight_IsTelnet()
        {
            const char responseCode = '8';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.Telnet, responseType);
        }

        [Test]
        public void ResponseType_Nine_IsBinary()
        {
            const char responseCode = '9';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.Binary, responseType);
        }

        [Test]
        public void ResponseType_Plus_IsRedundantServer()
        {
            const char responseCode = '+';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.RedundantServer, responseType);
        }

        [Test]
        public void ResponseType_UpperT_IsTN3270()
        {
            const char responseCode = 'T';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.TN3270, responseType);
        }

        [Test]
        public void ResponseType_LowerG_IsGIF()
        {
            const char responseCode = 'g';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.GIF, responseType);
        }

        [Test]
        public void ResponseType_UpperI_IsImage()
        {
            const char responseCode = 'I';

            var responseType = (ResponseType)responseCode;

            Assert.AreEqual(ResponseType.Image, responseType);
        }
    }
}
