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
    public class StringCutterTests
    {
        [TestMethod]
        public void ShortensString()
        {
            var cutter = new StringCutter();

            cutter.Cut("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum diam nisl sit amet erat. Duis semper. Duis arcu massa, scelerisque vitae, consequat in, pretium a, enim. Pellentesque congue. Ut in risus volutpat libero pharetra tempor. Cras vestibulum bibendum augue. Praesent egestas leo in pede. Praesent blandit odio eu enim. Pellentesque sed dui ut augue blandit sodales. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aliquam nibh. Mauris ac mauris sed pede pellentesque fermentum. Maecenas adipiscing ante non diam sodales hendrerit. "
                    , 150)
                .ShouldBe("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed,...");
        }

        [TestMethod]
        public void LeavesShortStringsUntouched()
        {
            var cutter = new StringCutter();

            cutter.Cut("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", 150)
                .ShouldBe("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
        }

        [TestMethod]
        public void NullStringsAreLeftAsNull()
        {
            var cutter = new StringCutter();

            cutter.Cut(null, 150)
                .ShouldBeNull();
        }
    }
}
