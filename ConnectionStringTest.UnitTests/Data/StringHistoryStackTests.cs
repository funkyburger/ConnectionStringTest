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
            var stack = new StringHistoryStack();

            stack.Stack("titi", 0, 4);
            stack.Stack("toto", 0, 4);
            stack.Stack("tata", 0, 4);

            stack.ShouldBe(new HistoryStackItem[] { 
                new HistoryStackItem(string.Empty, 0, 0),
                new HistoryStackItem("titi", 0, 4),
                new HistoryStackItem("toto", 0, 4),
                new HistoryStackItem("tata", 0, 4),
            });
            stack.Current.ShouldBe(new HistoryStackItem("tata", 0, 4));
        }

        [TestMethod]
        public void SimilarStringsDontGetStacked()
        {
            var stack = new StringHistoryStack();

            stack.Stack("titi", 0, 4);
            stack.Stack("titi", 0, 2);
            stack.Stack("titi", 1, 4);

            stack.ShouldBe(new HistoryStackItem[] {
                new HistoryStackItem(string.Empty, 0, 0),
                new HistoryStackItem("titi", 0, 4) 
            });
        }

        [TestMethod]
        public void StringCanBeUndone()
        {
            var stack = new StringHistoryStack();

            stack.Stack("titi", 0, 4);
            stack.Stack("toto", 0, 4);
            stack.Stack("tata", 0, 4);

            stack.Undo().ShouldBe(new HistoryStackItem("toto", 0, 4));
            stack.Current.ShouldBe(new HistoryStackItem("toto", 0, 4));

            stack.Undo().ShouldBe(new HistoryStackItem("titi", 0, 4));
            stack.Current.ShouldBe(new HistoryStackItem("titi", 0, 4));

            stack.Undo().ShouldBe(new HistoryStackItem(string.Empty, 0, 0));
            stack.Current.ShouldBe(new HistoryStackItem(string.Empty, 0, 0));

            stack.Undo().ShouldBe(new HistoryStackItem(string.Empty, 0, 0));
            stack.Current.ShouldBe(new HistoryStackItem(string.Empty, 0, 0));

            stack.Undo().ShouldBe(new HistoryStackItem(string.Empty, 0, 0));
            stack.Current.ShouldBe(new HistoryStackItem(string.Empty, 0, 0));
        }

        [TestMethod]
        public void StringCanBeRedone()
        {
            var stack = new StringHistoryStack();

            stack.Stack("titi", 0, 4);
            stack.Stack("toto", 0, 4);
            stack.Stack("tata", 0, 4);

            stack.Undo();
            stack.Undo().ShouldBe(new HistoryStackItem("titi", 0, 4));
            stack.Current.ShouldBe(new HistoryStackItem("titi", 0, 4));

            stack.Undo().ShouldBe(new HistoryStackItem(string.Empty, 0, 0));
            stack.Current.ShouldBe(new HistoryStackItem(string.Empty, 0, 0));

            stack.Undo().ShouldBe(new HistoryStackItem(string.Empty, 0, 0));
            stack.Current.ShouldBe(new HistoryStackItem(string.Empty, 0, 0));
        }

        [TestMethod]
        public void SavingInMiddleOfHistoryResetsComingOnes()
        {
            var stack = new StringHistoryStack();

            stack.Stack("titi", 0, 4);
            stack.Stack("toto", 0, 4);
            stack.Stack("tata", 0, 4);

            stack.Undo();
            stack.Undo();
            stack.Stack("bidule", 0, 6);

            stack.ShouldBe(new HistoryStackItem[] {
                new HistoryStackItem(string.Empty, 0, 0),
                new HistoryStackItem("titi", 0, 4),
                new HistoryStackItem("bidule", 0, 6)
            });
        }

        [TestMethod]
        public void EquivalentStackItemsAreEqual()
        {
            var item1 = new HistoryStackItem("titi", 0, 4);
            var item2 = item1;

            (item1 == item2).ShouldBeTrue();

            item2 = new HistoryStackItem("titi", 0, 4);

            (item1 == item2).ShouldBeTrue();
        }

        [TestMethod]
        public void NullStackItemsAreEqualToNull()
        {
            var item1 = new HistoryStackItem("titi", 0, 4);
            HistoryStackItem item2 = null;

            (item1 == null).ShouldBeFalse();
            (item2 == null).ShouldBeTrue();
            (item1 == item2).ShouldBeFalse();
            (item2 == item1).ShouldBeFalse();
        }

        [TestMethod]
        public void NonEquivalentStackItemsArentEqual()
        {
            var item1 = new HistoryStackItem("titi", 0, 4);
            var item2 = new HistoryStackItem("moto", 0, 4);

            (item1 == item2).ShouldBeFalse();

            item2 = new HistoryStackItem("titi", 1, 4);

            (item1 == item2).ShouldBeFalse();

            item2 = new HistoryStackItem("titi", 0, 3);

            (item1 == item2).ShouldBeFalse();
        }
    }
}
