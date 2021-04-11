using ConnectionStringTest.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using ConnectionStringTest.EventHandling;
using ConnectionStringTest.Utils;
using ConnectionStringTest.UI;
using ConnectionStringTest.Exceptions;

namespace ConnectionStringTest.UnitTests.EventHandling
{
    [TestClass]
    public class TestFiredHandlerTests
    {
        [TestMethod]
        public void HandlerDisplaysResultsOnSuccess()
        {
            var stringTesterMock = new Mock<IConnectionStringTester>();
            stringTesterMock.Setup(t => t.Test(It.IsAny<string>()))
                .ReturnsAsync(() => new TestResponse(true, "success!"));

            var stringCleanerMock = new Mock<IConnectionStringCleaner>();

            var mainControlMock = new Mock<IMainTestControl>();
            var linkedToMainTestControlMock = new Mock<ILinkedToMainTestControl>();
            linkedToMainTestControlMock.Setup(l => l.MainTestControl).Returns(mainControlMock.Object);

            var handler = new TestFiredHandler(stringTesterMock.Object, stringCleanerMock.Object);

            handler.Handle(Event.TestFired, linkedToMainTestControlMock.Object);

            // Wait until the test is over
            while (handler.TestPending) { }
            
            mainControlMock.Verify(c => c.SetStatus(TestStatus.Succeeded), Times.Once);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), true), Times.Once);
        }

        [TestMethod]
        public void HandlerDisplaysResultsOnFailure()
        {
            var stringTesterMock = new Mock<IConnectionStringTester>();
            stringTesterMock.Setup(t => t.Test(It.IsAny<string>()))
                .ReturnsAsync(() => new TestResponse(false, "failure!"));
            
            var stringCleanerMock = new Mock<IConnectionStringCleaner>();

            var mainControlMock = new Mock<IMainTestControl>();
            var linkedToMainTestControlMock = new Mock<ILinkedToMainTestControl>();
            linkedToMainTestControlMock.Setup(l => l.MainTestControl).Returns(mainControlMock.Object);

            var handler = new TestFiredHandler(stringTesterMock.Object, stringCleanerMock.Object);

            handler.Handle(Event.TestFired, linkedToMainTestControlMock.Object);

            // Wait until the test is over
            while (handler.TestPending) { }

            mainControlMock.Verify(c => c.SetStatus(TestStatus.Failed), Times.Once);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), false), Times.Once);
        }

        [TestMethod]
        public void HandlerCancelsTheTestProperly()
        {
            var stringCleanerMock = new Mock<IConnectionStringCleaner>();

            var mainControlMock = new Mock<IMainTestControl>();
            var linkedToMainTestControlMock = new Mock<ILinkedToMainTestControl>();
            linkedToMainTestControlMock.Setup(l => l.MainTestControl).Returns(mainControlMock.Object);

            var handler = new TestFiredHandler(new DelayedConnectionStringTester(), stringCleanerMock.Object);

            handler.Handle(Event.TestFired, linkedToMainTestControlMock.Object);
            handler.Handle(Event.TestCancelled, linkedToMainTestControlMock.Object);

            // Wait until the test is over
            while (handler.TestPending) { }

            mainControlMock.Verify(c => c.SetStatus(TestStatus.Succeeded), Times.Never);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), true), Times.Never);

            mainControlMock.Verify(c => c.SetStatus(TestStatus.Cancelled), Times.Once);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), false), Times.Once);
        }

        [TestMethod]
        public void HandlerThrowsExceptionOnTestingWhenNotIdle()
        {
            var exceptionThrown = false;

            var stringTesterMock = new Mock<IConnectionStringTester>();
            stringTesterMock.Setup(t => t.Test(It.IsAny<string>()))
                .ReturnsAsync(() => new TestResponse(true, "success!"));

            var stringCleanerMock = new Mock<IConnectionStringCleaner>();

            var mainControlMock = new Mock<IMainTestControl>();
            var linkedToMainTestControlMock = new Mock<ILinkedToMainTestControl>();
            linkedToMainTestControlMock.Setup(l => l.MainTestControl).Returns(mainControlMock.Object);

            var handler = new TestFiredHandler(stringTesterMock.Object, stringCleanerMock.Object);

            handler.Handle(Event.TestFired, linkedToMainTestControlMock.Object);
            try
            {
                handler.Handle(Event.TestFired, linkedToMainTestControlMock.Object);
            }
            catch (TestAlreadyRunningException)
            {
                exceptionThrown = true;
            }

            exceptionThrown.ShouldBeTrue();
        }

        private class DelayedConnectionStringTester : IConnectionStringTester
        {
            public async Task<TestResponse> Test(string connectionString)
            {
                await Task.Delay(1000);
                return new TestResponse(true, "too long, too bad ...");
            }
        }
    }
}
