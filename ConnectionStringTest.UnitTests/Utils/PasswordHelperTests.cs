﻿using ConnectionStringTest.Data;
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
    public class PasswordHelperTests
    {
        [TestMethod]
        public void HidesPasswordIfNeeded()
        {
            var helper = new PasswordHelper();

            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●;");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\";")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●●\";");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$w0rd\"")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●●●●●\"");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$w0rd")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$w0rd\"        ;User Id=john.smith;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"●●●●●●●●●\"        ;User Id=john.smith;");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$w0rd        ;User Id=john.smith;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      ●●●●●●●●●        ;User Id=john.smith;");
        }

        [TestMethod]
        public void HidesPasswordWhileTyping()
        {
            var helper = new PasswordHelper();

            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=\"●●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      \"●●●●●");
            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      p4$$")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;          Password        =      ●●●●●");
        }

        [TestMethod]
        public void DoesntHidePasswordIfNotNeeded()
        {
            var helper = new PasswordHelper();

            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Initial Catalog=Blah;Connection Timeout=60;Integrated Security=True;MultipleActiveResultSets=True");
        }

        [TestMethod]
        public void AddMaskCharIfPassordIsAlreadyHidden()
        {
            var helper = new PasswordHelper();

            helper.Mask("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●;")
                .ShouldBe("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Blah.mdf;Connection Timeout=60;User Id=john.smith; Password=●●●●●●●●●●;");
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
    }
}