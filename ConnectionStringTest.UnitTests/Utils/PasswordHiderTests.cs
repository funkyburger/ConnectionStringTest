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

            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=p4$$w0rd;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=●●●●●●●●●;");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=\"p4$$w0rd\";")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=\"●●●●●●●●●\";");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=\"p4$$w0rd\"")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=\"●●●●●●●●●\"");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;User Id=episandbox; Password=●●●●●●●●●");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;          Password        =      \"p4$$w0rd\"        ;User Id=episandbox;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;          Password        =      \"●●●●●●●●●\"        ;User Id=episandbox;");
            hider.Hide("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;          Password        =      p4$$w0rd        ;User Id=episandbox;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Temp\\EpiSandbox.mdf;Connection Timeout=60;          Password        =      ●●●●●●●●●        ;User Id=episandbox;");
        }

        [TestMethod]
        public void DoesntHidePasswordIfNotNeeded()
        {
        }

        [TestMethod]
        public void LetStringUnchangedIfPassordIsAlreadyHidden()
        {
        }
    }
}
