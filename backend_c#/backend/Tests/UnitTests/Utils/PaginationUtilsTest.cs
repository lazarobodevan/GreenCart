using backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.UnitTests.Utils {
    public class PaginationUtilsTest {

        private const string _baseUrl = "http://localhost:8080/Product/producerId=123";

        [Theory]
        [InlineData(0, 2, $"{_baseUrl}?page=1")]
        [InlineData(1, 2, null)]
        public void GetNextUrl(int currentPage, int totalPages, string? expected) {
            //Arrange
            PaginationUtils paginationUtils = new PaginationUtils();

            //Act
            var result = paginationUtils.GetNextUrl(currentPage, totalPages, _baseUrl);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(1, $"{_baseUrl}?page=0")]
        [InlineData(2, $"{_baseUrl}?page=1")]
        public void GetPreviousUrl(int currentPage, string? expected) {
            //Arrange
            PaginationUtils paginationUtils = new PaginationUtils();

            //Act
            var result = paginationUtils.GetPreviousUrl(currentPage, _baseUrl);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
