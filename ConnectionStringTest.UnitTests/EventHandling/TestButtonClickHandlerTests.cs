﻿using ConnectionStringTest.Data;
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

namespace ConnectionStringTest.UnitTests.EventHandling
{
    [TestClass]
    public class TestButtonClickHandlerTests
    {
        [TestMethod]
        public async Task HandlerDisplaysResultsOnSuccess()
        {
            var stringTesterMock = new Mock<IConnectionStringTester>();
            stringTesterMock.Setup(t => t.Test(It.IsAny<string>()))
                .ReturnsAsync(() => new TestResponse(true, "success!"));

            var stringCleanerMock = new Mock<IConnectionStringCleaner>();

            var mainControlMock = new Mock<IMainTestControl>(); 

            var handler = new TestButtonClickHandler(stringTesterMock.Object, stringCleanerMock.Object);

            await handler.Handle(Event.TestButtonClicked, mainControlMock.Object);

            mainControlMock.Verify(c => c.SetStatus(TestStatus.Succeeded), Times.Once);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), true), Times.Once);
        }

        [TestMethod]
        public async Task HandlerDisplaysResultsOnFailure()
        {
            var stringTesterMock = new Mock<IConnectionStringTester>();
            stringTesterMock.Setup(t => t.Test(It.IsAny<string>()))
                .ReturnsAsync(() => new TestResponse(false, "failure!"));
            
            var stringCleanerMock = new Mock<IConnectionStringCleaner>();

            var mainControlMock = new Mock<IMainTestControl>();

            var handler = new TestButtonClickHandler(stringTesterMock.Object, stringCleanerMock.Object);

            await handler.Handle(Event.TestButtonClicked, mainControlMock.Object);

            mainControlMock.Verify(c => c.SetStatus(TestStatus.Failed), Times.Once);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), false), Times.Once);
        }

        [TestMethod]
        public async Task HandlerCancelsTheTestProperly()
        {
            var stringCleanerMock = new Mock<IConnectionStringCleaner>();

            var mainControlMock = new Mock<IMainTestControl>();

            var handler = new TestButtonClickHandler(new DelayedConnectionStringTester(), stringCleanerMock.Object);

            await handler.Handle(Event.TestButtonClicked, mainControlMock.Object);
            await handler.Handle(Event.TestCancelled, mainControlMock.Object);

            mainControlMock.Verify(c => c.SetStatus(TestStatus.Succeeded), Times.Never);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), true), Times.Never);

            mainControlMock.Verify(c => c.SetStatus(TestStatus.Cancelled), Times.Once);
            mainControlMock.Verify(c => c.DisplayMessage(It.IsAny<string>(), false), Times.Once);
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
