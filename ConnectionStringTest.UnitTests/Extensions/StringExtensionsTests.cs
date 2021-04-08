using ConnectionStringTest.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using ConnectionStringTest.Utils;

namespace ConnectionStringTest.UnitTests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void SegmentAreReplaced()
        {
            "toto titi tata tutu".OverwriteSegment("pouet", 5, 4)
                .ShouldBe("toto pouet tata tutu");
        }
    }
}
