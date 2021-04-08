using ConnectionStringTest.Data;
using ConnectionStringTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;

namespace ConnectionStringTest.UnitTests.Data
{
    [TestClass]
    public class HistoryServiceTests
    {
        [TestMethod]
        public void GeneratesAutoComplete()
        {
            var applicationDataServiceMock = new Mock<IApplicationDataService>();
            applicationDataServiceMock.Setup(s => s.GetApplicationData())
                .Returns(new ApplicationData(new string[] {
                    "titi",
                    "tata",
                    "toto"
                }));

            var passwordHelperMock = new Mock<IPasswordHelper>();
            passwordHelperMock.Setup(h => h.Mask(It.IsAny<string>()))
                .Returns<string>(s => s + "*masked*");

            var historyService = new HistoryService(applicationDataServiceMock.Object, passwordHelperMock.Object);

            var autoComplete = historyService.GetAutoComplete();

            autoComplete[0].ShouldBe("titi*masked*");
            autoComplete.GetUnmasked(autoComplete[0]).ShouldBe("titi");

            autoComplete[1].ShouldBe("tata*masked*");
            autoComplete.GetUnmasked(autoComplete[1]).ShouldBe("tata");

            autoComplete[2].ShouldBe("toto*masked*");
            autoComplete.GetUnmasked(autoComplete[2]).ShouldBe("toto");
        }

        [TestMethod]
        public void SimilarConnectionStringsAreStored()
        {
            var applicationDataServiceMock = new Mock<IApplicationDataService>();
            applicationDataServiceMock.Setup(s => s.GetApplicationData())
                .Returns(new ApplicationData(new string[] {
                    "toto",
                    "toto",
                    "toto"
                }));

            var passwordHelperMock = new Mock<IPasswordHelper>();
            passwordHelperMock.Setup(h => h.Mask(It.IsAny<string>()))
                .Returns<string>(s => s + "*masked*");
            passwordHelperMock.Setup(h => h.AddGarble(It.IsAny<string>()))
                .Returns<string>(s => s + "*");

            var historyService = new HistoryService(applicationDataServiceMock.Object, passwordHelperMock.Object);

            var autoComplete = historyService.GetAutoComplete();

            autoComplete[0].ShouldBe("toto*masked*");
            autoComplete.GetUnmasked(autoComplete[0]).ShouldBe("toto");

            autoComplete[1].ShouldBe("toto*masked**");
            autoComplete.GetUnmasked(autoComplete[1]).ShouldBe("toto");

            autoComplete[2].ShouldBe("toto*masked***");
            autoComplete.GetUnmasked(autoComplete[2]).ShouldBe("toto");
        }
    }
}
