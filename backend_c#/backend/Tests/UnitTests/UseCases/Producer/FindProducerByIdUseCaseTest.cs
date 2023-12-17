using backend.Contexts;
using backend.Models;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories;

namespace Tests.UnitTests.UseCases.Producer
{
    public class FindProducerByIdUseCaseTest {

        private readonly Mock<IProducerRepository> _producerRepository;
        private ProducerDTOFactory producerFactory = new ProducerDTOFactory();

        public FindProducerByIdUseCaseTest() {
            _producerRepository = new Mock<IProducerRepository>();
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProducerId_ReturnsProducer() {
            //Arrange
            var producerId = Guid.NewGuid();
            _producerRepository.Setup(x => x.Save(It.IsAny<backend.Models.Producer>())).ReturnsAsync(new backend.Models.Producer {Id = producerId});
            _producerRepository.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(new backend.Models.Producer { Id = producerId });

            CreateProducerUseCase createProducerUseCase = new CreateProducerUseCase(_producerRepository.Object);
            FindProducerByIdUseCase findProducerByIdUseCase = new FindProducerByIdUseCase(_producerRepository.Object);

            var producer = producerFactory.GetDefaultCreateProducerDto();

            //Act
            var createdProducer = await createProducerUseCase.Execute(producer);
            var foundProducer = await findProducerByIdUseCase.Execute(createdProducer.Id);

            //Assert
            Assert.NotNull(foundProducer);
            Assert.Equal(createdProducer.Id, foundProducer.Id);
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenNotExistantId_ReturnsNull() {

            //Arrange
            _producerRepository.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<backend.Models.Producer?>(null));
            FindProducerByIdUseCase findProducerByIdUseCase = new FindProducerByIdUseCase(_producerRepository.Object);

            //Act
            var foundProducer = await findProducerByIdUseCase.Execute(Guid.NewGuid());

            //Assert
            Assert.Null(foundProducer);
        }
    }
}
