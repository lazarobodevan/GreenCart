using backend.Contexts;
using backend.Models;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using backend.ProducerPicture.Services;
using backend.Shared.Services.Location;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories.Producer;

namespace Tests.UnitTests.UseCases.Producer
{
    public class FindProducerByIdUseCaseTest {

        private readonly Mock<IProducerRepository> _producerRepository;
        private readonly Mock<IProducerPictureService> _producerPictureServiceMock;
        private readonly Mock<ILocationService> locationService;
        private ProducerDTOFactory producerFactory = new ProducerDTOFactory();

        public FindProducerByIdUseCaseTest() {
            _producerRepository = new Mock<IProducerRepository>();
            _producerPictureServiceMock = new Mock<IProducerPictureService>();
            locationService = new Mock<ILocationService>();
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProducerId_ReturnsProducer() {
            //Arrange
            var producerId = Guid.NewGuid();
            _producerRepository.Setup(x => x.Save(It.IsAny<backend.Models.Producer>())).ReturnsAsync(new ProducerFactory().WithId(producerId).Build());
            _producerRepository.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(new ProducerFactory().WithId(producerId).Build());
            _producerPictureServiceMock.Setup(x => x.GetProfilePictureAsync(It.IsAny<backend.Models.Producer>())).ReturnsAsync("link");
            locationService.Setup(x => x.GetLocationByLatLon(It.IsAny<double>(), It.IsAny<double>())).Returns(new Location() { Address = "", City = "", State = "", UserId = Guid.NewGuid(), ZipCode = "" });

            CreateProducerUseCase createProducerUseCase = new CreateProducerUseCase(_producerRepository.Object, _producerPictureServiceMock.Object, locationService.Object);
            FindProducerByIdUseCase findProducerByIdUseCase = new FindProducerByIdUseCase(_producerRepository.Object, _producerPictureServiceMock.Object);

            var producer = producerFactory.Build();

            //Act
            var createdProducer = await createProducerUseCase.Execute(producer);
            var foundProducer = await findProducerByIdUseCase.Execute(createdProducer.Id);

            //Assert
            Assert.NotNull(foundProducer);
            Assert.Equal(createdProducer.Id, foundProducer.Id);
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenNotExistentId_ReturnsNull() {

            //Arrange
            _producerRepository.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<backend.Models.Producer?>(null));
            _producerPictureServiceMock.Setup(x => x.GetProfilePictureAsync(It.IsAny<backend.Models.Producer>())).ReturnsAsync("link");

            FindProducerByIdUseCase findProducerByIdUseCase = new FindProducerByIdUseCase(_producerRepository.Object, _producerPictureServiceMock.Object);

            //Act
            var foundProducer = await findProducerByIdUseCase.Execute(Guid.NewGuid());

            //Assert
            Assert.Null(foundProducer);
        }
    }
}
