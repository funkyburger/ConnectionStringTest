using ConnectionStringTest.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;

namespace ConnectionStringTest.UnitTests.Data
{
    [TestClass]
    public class ApplicationDataServiceTests
    {
        [TestMethod]
        public void ServiceGetData()
        {
            var fileService = new Mock<IFileService>();
            var serializer = new Mock<IApplicationDataSerializer>();

            serializer.Setup(s => s.Deserialize(It.IsAny<string>()))
                .Returns(new ApplicationData(new string[] { "toto", "titi", "tata" }));

            var service = new ApplicationDataService(fileService.Object, serializer.Object);

            var data = service.GetApplicationData();

            data.History.ShouldBe(new string[] { "toto", "titi", "tata" });
        }

        [TestMethod]
        public void ServiceUsesCache()
        {
            var fileService = new Mock<IFileService>();
            var serializer = new Mock<IApplicationDataSerializer>();

            serializer.Setup(s => s.Deserialize(It.IsAny<string>()))
                .Returns(new ApplicationData(new string[] { "toto", "titi", "tata" }));

            var service = new ApplicationDataService(fileService.Object, serializer.Object);

            var data = service.GetApplicationData();

            data = service.GetApplicationData();

            fileService.Verify(s => s.LoadApplicationDataFileContent(), Times.Once);
        }

        [TestMethod]
        public void UpdateClearasCache()
        {
            var fileService = new Mock<IFileService>();
            var serializer = new Mock<IApplicationDataSerializer>();

            serializer.Setup(s => s.Deserialize(It.IsAny<string>()))
                .Returns(new ApplicationData(new string[] { "toto", "titi", "tata" }));

            var service = new ApplicationDataService(fileService.Object, serializer.Object);

            var data = service.GetApplicationData();

            service.UpdateApplicationData(new ApplicationData(new string[] { }));

            data = service.GetApplicationData();

            fileService.Verify(s => s.LoadApplicationDataFileContent(), Times.Exactly(2));
        }
    }
}
