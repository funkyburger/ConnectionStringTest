using ConnectionStringTest.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using ConnectionStringTest.Utils;

namespace ConnectionStringTest.UnitTests.Utils
{
    [TestClass]
    public class ConnectionStringCleanerTests
    {
        [TestMethod]
        public void EscapeCharactersAreReplaced()
        {
            var cleaner = new ConnectionStringCleaner();

            cleaner.Clean("Data Source=(LocalDb)\\\\MSSQLLocalDB;AttachDbFilename=\\\"C:\\\\Temp\\\\blah.mdf\\\";Initial Catalog=blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Temp\\blah.mdf\";Initial Catalog=blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True");
        }

        [TestMethod]
        public void CleanLetStringIntact()
        {
            var cleaner = new ConnectionStringCleaner();

            cleaner.Clean("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Temp\\blah.mdf\";Initial Catalog=blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Temp\\blah.mdf\";Initial Catalog=blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True");
        }

        [TestMethod]
        public void NullsDontBreak()
        {
            var cleaner = new ConnectionStringCleaner();
            cleaner.Clean(null).ShouldBeNull();
            cleaner.Clean(string.Empty).ShouldBeNull();
        }
    }
}
