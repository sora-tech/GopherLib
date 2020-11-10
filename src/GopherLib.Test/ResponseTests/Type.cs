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
        public void ItemType_Unknown_NotKnown()
        {
            //Given the list of supported types from the RFC
            //the value of Uknown must not be part of them

            var knownTypes = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', 'T', 'g', 'I' };

            var unknown = ItemType.Unknown;

            CollectionAssert.DoesNotContain(knownTypes, unknown);
        }

        [Test]
        public void ItemType_Unknown_NotReserved()
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

            var unknown = ItemType.Unknown;

            CollectionAssert.DoesNotContain(knownTypes, unknown);
        }

        [Test]
        public void ItemType_Zero_IsFile()
        {
            const char responseCode = '0';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.File, itemType);
        }

        [Test]
        public void ItemType_One_IsDirectory()
        {
            const char responseCode = '1';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.Directory, itemType);
        }

        [Test]
        public void ItemType_Two_IsPhoneBook()
        {
            const char responseCode = '2';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.PhoneBook, itemType);
        }

        [Test]
        public void ItemType_Three_IsError()
        {
            const char responseCode = '3';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.Error, itemType);
        }

        [Test]
        public void ItemType_Four_IsBinHexed()
        {
            const char responseCode = '4';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.BinHexed, itemType);
        }

        [Test]
        public void ItemType_Five_IsDOSBinary()
        {
            const char responseCode = '5';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.DOSBinary, itemType);
        }

        [Test]
        public void ItemType_Six_IsUUEncoded()
        {
            const char responseCode = '6';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.UUEncoded, itemType);
        }

        [Test]
        public void ItemType_Seven_IsIndexSearch()
        {
            const char responseCode = '7';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.IndexSearch, itemType);
        }

        [Test]
        public void ItemType_Eight_IsTelnet()
        {
            const char responseCode = '8';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.Telnet, itemType);
        }

        [Test]
        public void ItemType_Nine_IsBinary()
        {
            const char responseCode = '9';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.Binary, itemType);
        }

        [Test]
        public void ItemType_Plus_IsRedundantServer()
        {
            const char responseCode = '+';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.RedundantServer, itemType);
        }

        [Test]
        public void ItemType_UpperT_IsTN3270()
        {
            const char responseCode = 'T';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.TN3270, itemType);
        }

        [Test]
        public void ItemType_LowerG_IsGIF()
        {
            const char responseCode = 'g';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.GIF, itemType);
        }

        [Test]
        public void ItemType_UpperI_IsImage()
        {
            const char responseCode = 'I';

            var itemType = (ItemType)responseCode;

            Assert.AreEqual(ItemType.Image, itemType);
        }
    }
}
