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
    public class ConnectionStringStoreTests
    {
        [TestMethod]
        public void PasswordsAreMasked()
        {
            var applicationDataServiceMock = new Mock<IApplicationDataService>();
            applicationDataServiceMock.Setup(s => s.GetApplicationData())
                .Returns(new ApplicationData(new string[] {
                    "titi",
                    "tata",
                    "toto"
                }));

            var passwordMaskerMock = new Mock<IPasswordMasker>();
            passwordMaskerMock.Setup(m => m.Mask(It.IsAny<string>()))
                .Returns<string>(s => s + "*masked*");

            var store = new ConnectionStringStore(applicationDataServiceMock.Object, passwordMaskerMock.Object);
            store.GetConnectionStrings().ShouldAllBe(s => s.EndsWith("*masked*"));
        }

        [TestMethod]
        public void MaskedPasswordAreStoredAndCanBeRetrieved()
        {
            var applicationDataServiceMock = new Mock<IApplicationDataService>();
            applicationDataServiceMock.Setup(s => s.GetApplicationData())
                .Returns(new ApplicationData(new string[] {
                    "toto;Password=qwerty1",
                    "toto;Password=qwerty2",
                    "toto;Password=qwerty3"
                }));

            var passwordMaskerMock = new Mock<IPasswordMasker>();
            passwordMaskerMock.Setup(m => m.Mask(It.IsAny<string>()))
                .Returns<string>(s => {
                    var splitIndex = s.IndexOf('=');
                    return s.Substring(0, splitIndex + 1).PadRight(s.Length + 1, '*');
                });

            var store = new ConnectionStringStore(applicationDataServiceMock.Object, passwordMaskerMock.Object);
            var connectionStrings = store.GetConnectionStrings().ToArray();

            store.GetConnectionStringWithPassword(connectionStrings.First()).ShouldBe("toto;Password=qwerty1");
            store.GetConnectionStringWithPassword(connectionStrings.Skip(1).First()).ShouldBe("toto;Password=qwerty2");
            store.GetConnectionStringWithPassword(connectionStrings.Skip(2).First()).ShouldBe("toto;Password=qwerty3");
        }
    }
}
