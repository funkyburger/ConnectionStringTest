using ConnectionStringTest.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;

namespace ConnectionStringTest.UnitTests.Data
{
    [TestClass]
    public class StringHistoryStackTests
    {
        [TestMethod]
        public void HistoryGetsStacked()
        {
            var stack = new StringHistoryStack(string.Empty);

            stack.Stack("titi");
            stack.Stack("toto");
            stack.Stack("tata");

            stack.ShouldBe(new string[] { string.Empty, "titi", "toto", "tata" });
            stack.Current.ShouldBe("tata");
        }

        [TestMethod]
        public void SimilarStringsDontGetStacked()
        {
            var stack = new StringHistoryStack(string.Empty);

            stack.Stack("titi");
            stack.Stack("titi");
            stack.Stack("titi");

            stack.ShouldBe(new string[] { string.Empty, "titi" });
        }

        [TestMethod]
        public void StringCanBeUndone()
        {
            var stack = new StringHistoryStack(string.Empty);

            stack.Stack("titi");
            stack.Stack("toto");
            stack.Stack("tata");

            stack.Undo().ShouldBe("toto");
            stack.Current.ShouldBe("toto");

            stack.Undo().ShouldBe("titi");
            stack.Current.ShouldBe("titi");

            stack.Undo().ShouldBe(string.Empty);
            stack.Current.ShouldBe(string.Empty);

            stack.Undo().ShouldBe(string.Empty);
            stack.Current.ShouldBe(string.Empty);

            stack.Undo().ShouldBe(string.Empty);
            stack.Current.ShouldBe(string.Empty);
        }

        [TestMethod]
        public void StringCanBeRedone()
        {
            var stack = new StringHistoryStack(string.Empty);

            stack.Stack("titi");
            stack.Stack("toto");
            stack.Stack("tata");

            stack.Undo();
            stack.Undo();
            stack.Redo().ShouldBe("toto");
            stack.Current.ShouldBe("toto");

            stack.Redo().ShouldBe("tata");
            stack.Current.ShouldBe("tata");

            stack.Redo().ShouldBe("tata");
            stack.Current.ShouldBe("tata");
        }

        [TestMethod]
        public void SavingInMiddleOfHistoryResetsComingOnes()
        {
            var stack = new StringHistoryStack(string.Empty);

            stack.Stack("titi");
            stack.Stack("toto");
            stack.Stack("tata");

            stack.Undo();
            stack.Undo();
            stack.Stack("bidule");

            stack.ShouldBe(new string[] { string.Empty, "titi", "bidule" });
            stack.Current.ShouldBe("bidule");
        }
    }
}
