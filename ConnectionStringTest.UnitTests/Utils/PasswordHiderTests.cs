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
    public class PasswordHiderTests
    {
        [TestMethod]
        public void HidesPasswordIfNeeded()
        {
            var hider = new PasswordHider();

            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●;");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\";")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●●\";");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\"")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●●\"");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$w0rd\"        ;User Id=john.smith;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"●●●●●●●●●\"        ;User Id=john.smith;");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$w0rd        ;User Id=john.smith;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      ●●●●●●●●●        ;User Id=john.smith;");
        }

        [TestMethod]
        public void DoesntHidePasswordIfNotNeeded()
        {
            var hider = new PasswordHider();

            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True");
        }

        [TestMethod]
        public void AddMaskCharIfPassordIsAlreadyHidden()
        {
            var hider = new PasswordHider();

            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●●;");
        }
    }
}
