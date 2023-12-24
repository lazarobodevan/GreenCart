using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Exceptions;
using backend.Utils.Errors;

namespace UnitTests.UnitTests.Utils {
    public class ExceptionUtilsTest {

        [Fact]
        public void FormatException_GivenException_ReturnsFormatedExceptionResponse(){
            //Arrange
            var exception = new Exception("Exception test");

            //Act
            var formatted = ExceptionUtils.FormatExceptionResponse(exception);

            //Assert
            Assert.IsType<ExceptionResponseModel>(formatted);
            Assert.Equal("Exception test", $"{formatted.Error.Message}");
            Assert.Equal("System.Exception", formatted.Error.Type);
        }
    }
}
