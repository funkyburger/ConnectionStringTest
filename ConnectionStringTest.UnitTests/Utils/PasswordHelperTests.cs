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
using ConnectionStringTest.Exceptions;

namespace ConnectionStringTest.UnitTests.Utils
{
    [TestClass]
    public class PasswordHelperTests
    {
        [TestMethod]
        public void HidesPasswordIfNeeded()
        {
            var helper = new PasswordHelper();

            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●;");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\";")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●\";");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\"")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●\"");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$w0rd\"        ;User Id=john.smith;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"●●●●●●●●\"        ;User Id=john.smith;");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$w0rd        ;User Id=john.smith;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      ●●●●●●●●        ;User Id=john.smith;");
        }

        [TestMethod]
        public void HidesPasswordWhileTyping()
        {
            var helper = new PasswordHelper();

            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      ●●●●");
        }

        [TestMethod]
        public void DoesntHidePasswordIfNotNeeded()
        {
            var helper = new PasswordHelper();

            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True");
        }

        [TestMethod]
        public void ExtractedPasswordIsNullIfNone()
        {
            var helper = new PasswordHelper();

            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True")
                .ShouldBeNull();
        }

        [TestMethod]
        public void ExtractsPasswords()
        {
            var helper = new PasswordHelper();

            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd;")
                .ShouldBe("p4$$w0rd");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\";")
                .ShouldBe("p4$$w0rd");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\"")
                .ShouldBe("p4$$w0rd");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd")
                .ShouldBe("p4$$w0rd");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$w0rd\"        ;User Id=john.smith;")
                .ShouldBe("p4$$w0rd");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$w0rd        ;User Id=john.smith;")
                .ShouldBe("p4$$w0rd");
        }

        [TestMethod]
        public void ExtractsPasswordsWhileTyping()
        {
            var helper = new PasswordHelper();

            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$")
                .ShouldBe("p4$$");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$")
                .ShouldBe("p4$$");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$")
                .ShouldBe("p4$$");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$")
                .ShouldBe("p4$$");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$")
                .ShouldBe("p4$$");
            helper.ExtractPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$")
                .ShouldBe("p4$$");
        }

        [TestMethod]
        public void PasswordGetsUnmasked()
        {
            var helper = new PasswordHelper();

            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●;", "p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd;");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●●\";", "p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\";", "p4$$w0rd");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●●\"", "p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\"");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●", "p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"●●●●●●●●●\"        ;User Id=john.smith;", "p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$w0rd\"        ;User Id=john.smith;");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      ●●●●●●●●●        ;User Id=john.smith;", "p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$w0rd        ;User Id=john.smith;");
        }

        [TestMethod]
        public void PasswordGetsUnmaskedWhileTyping()
        {
            var helper = new PasswordHelper();

            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●", "p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●", "p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●", "p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●", "p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"●●●●●", "p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$");
            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      ●●●●●", "p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$");
        }

        [TestMethod]
        public void PasswordThrowsExceptionIfNoSlot()
        {
            var exceptionThrown = false;
            var helper = new PasswordHelper();

            try
            {
                helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True", "p4$$w0rd");
            }
            catch (PasswordException)
            {
                exceptionThrown = true;
            }

            exceptionThrown.ShouldBeTrue();
        }

        [TestMethod]
        public void NullPasswordAreOkIfNoSlot()
        {
            var helper = new PasswordHelper();

            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True", null)
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True");
        }

        [TestMethod]
        public void NullPasswordAreSetAsEmpty()
        {
            var helper = new PasswordHelper();

            helper.SetPassword("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●;", null)
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=;");
        }

        [TestMethod]
        public void MaskedPasswordAreUpdated()
        {
            var helper = new PasswordHelper();

            helper.UpdatePassword("p4$$w0r", "●●●●●●●d").ShouldBe("p4$$w0rd");
            helper.UpdatePassword("p4$$w0", "●●●●●●rd").ShouldBe("p4$$w0rd");
            helper.UpdatePassword("p4$$w0rd", "●●●●●●●").ShouldBe("p4$$w0r");
            helper.UpdatePassword("p4$$w0rd", "●●●●●●").ShouldBe("p4$$w0");
            helper.UpdatePassword("p4$$w0rd", string.Empty).ShouldBeNull();
            helper.UpdatePassword("p4$$w0rd", null).ShouldBeNull();
        }

        [TestMethod]
        public void UnmaskedPasswordWithGarbleThrowsException()
        {
            var exceptionThrown = false;
            var helper = new PasswordHelper();

            try
            {
                helper.UpdatePassword("p4$$w0r", "●●●●●●●●d");
            }
            catch (PasswordException)
            {
                exceptionThrown = true;
            }

            exceptionThrown.ShouldBeTrue();
        }

        [TestMethod]
        public void UnmaskedPasswordNotMatchingThrowsException()
        {
            var exceptionThrown = false;
            var helper = new PasswordHelper();

            try
            {
                helper.UpdatePassword("p4$$w0r", "●●●●●●ad");
            }
            catch (PasswordException)
            {
                exceptionThrown = true;
            }

            exceptionThrown.ShouldBeTrue();
        }
    }
}
