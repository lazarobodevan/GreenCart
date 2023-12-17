using backend.Producer.Repository;
using backend.Producer.UseCases;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases.Producer
{
    public class FindNearProducersUseCaseTest {
        private Mock<IProducerRepository> _producerRepositoryMock;

        public FindNearProducersUseCaseTest() { 
            _producerRepositoryMock = new Mock<IProducerRepository>();
        }

        [Fact]
        [Trait("OP","FindNearProducers")]
        public void FindNearProducers_GivenCityName_ReturnsListOfProducers() {

            //Arrange
            _producerRepositoryMock.Setup(x => x.FindNearProducers(It.IsAny<string>())).Returns(new List<backend.Models.Producer> {
                new backend.Models.Producer(),
                new backend.Models.Producer(),
            });
            FindNearProducersUseCase findNearProducersUseCase = new FindNearProducersUseCase(_producerRepositoryMock.Object);

            //Act
            var foundProducers = findNearProducersUseCase.Execute(It.IsAny<string>());

            //Assert
            Assert.Equal(2, foundProducers.Count());
        }

        [Fact]
        [Trait("OP", "FindNearProducers")]
        public void FindNearProducers_GivenCityName_ReturnsEmptyListOfProducers() {
            //Arrange
            _producerRepositoryMock.Setup(x => x.FindNearProducers(It.IsAny<string>())).Returns(new List<backend.Models.Producer>());
            FindNearProducersUseCase findNearProducersUseCase = new FindNearProducersUseCase(_producerRepositoryMock.Object);

            //Act
            var foundProducers = findNearProducersUseCase.Execute(It.IsAny<string>());

            //Assert
            Assert.NotNull(foundProducers);
            Assert.Empty(foundProducers);
        }
    }
}
