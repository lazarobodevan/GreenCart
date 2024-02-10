using backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.UnitTests.Utils {
    public class MathUtilsTest {
        [Fact]
        public void CalcNewAverageBasedOnPreviousAverage_GivenPreviousAverage_ReturnsNewAverage() {
            //Arrange

            /*
             * 1, 2      -> 1,5
             * 1, 2, 3   -> 2
            */

            double prevAvg = 1.5;
            int prevElemCount = 2;
            int sumNewElem = 3;
            int newElemCount = 1;

            //Act

            var newAvg = MathUtils.CalcAverageBasedOnPreviousAverage(prevAvg, prevElemCount, sumNewElem, newElemCount);

            //Assert
            Assert.Equal(2, newAvg);
        }
    }
}
