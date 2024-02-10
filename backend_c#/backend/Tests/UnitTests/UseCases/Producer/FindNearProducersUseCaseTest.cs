using backend.Producer.Repository;
using backend.Producer.UseCases;
using backend.ProducerPicture.Services;
using backend.Shared.Classes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories.Producer;

namespace Tests.UnitTests.UseCases.Producer
{
    public class FindNearProducersUseCaseTest {
        private Mock<IProducerRepository> _producerRepositoryMock;
        private Mock<IProducerPictureService> _producerPictureService;

        public FindNearProducersUseCaseTest() { 
            _producerRepositoryMock = new Mock<IProducerRepository>();
            _producerPictureService = new Mock<IProducerPictureService>();
        }

        [Fact]
        [Trait("OP","FindNearProducers")]
        public async Task FindNearProducers_GivenMyLocation_ReturnsListOfProducers() {

            //Arrange
            _producerRepositoryMock.Setup(x => x.FindNearProducers(It.IsAny<Location>(), It.IsAny<int>(), It.IsAny<int>(), null)).Returns(new Pagination<backend.Models.Producer> {
                Data = new List<backend.Models.Producer>(){
                    new ProducerFactory().Build(),
                    new ProducerFactory().Build(),
                }
            });
            _producerPictureService.Setup(x => x.GetProfilePictureAsync(It.IsAny<backend.Models.Producer>())).ReturnsAsync("link");
            FindNearProducersUseCase findNearProducersUseCase = new FindNearProducersUseCase(_producerRepositoryMock.Object, _producerPictureService.Object);

            //Act
            var foundProducers = await findNearProducersUseCase.Execute(It.IsAny<Location>(), It.IsAny<int>(), It.IsAny<int>(), null);

            //Assert
            Assert.Equal(2, foundProducers.Data.Count());
        }

        [Fact]
        [Trait("OP", "FindNearProducers")]
        public async Task FindNearProducers_GivenCityName_ReturnsEmptyListOfProducers() {
            //Arrange
            _producerRepositoryMock.Setup(x => x.FindNearProducers(It.IsAny<Location>(), It.IsAny<int>(), It.IsAny<int>(), null)).Returns(new Pagination<backend.Models.Producer>());
            _producerPictureService.Setup(x => x.GetProfilePictureAsync(It.IsAny<backend.Models.Producer>())).ReturnsAsync("link");

            FindNearProducersUseCase findNearProducersUseCase = new FindNearProducersUseCase(_producerRepositoryMock.Object, _producerPictureService.Object);

            //Act
            var foundProducers = await findNearProducersUseCase.Execute(It.IsAny<Location>(), It.IsAny<int>(), It.IsAny<int>(), null);

            //Assert
            Assert.NotNull(foundProducers);
            Assert.Empty(foundProducers.Data);
        }
    }
}
