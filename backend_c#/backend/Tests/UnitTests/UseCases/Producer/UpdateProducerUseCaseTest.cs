using backend.Producer.DTOs;
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
    public class UpdateProducerUseCaseTest {

        private Mock<IProducerRepository> _producerRepositoryMock;

        public UpdateProducerUseCaseTest() {
            _producerRepositoryMock = new Mock<IProducerRepository>();
        }

        [Fact]
        [Trait("OP","Update")]
        public async Task Update_GivenProducer_ReturnsUpdatedProducer() {
            
            //Arrange
            var producerId = Guid.NewGuid();
            var producer = new backend.Models.Producer {
                Id = producerId,
                Name = "Producer Test"
            };
            var expectedProducer = new backend.Models.Producer {
                Id = producerId,
                Name = "Producer Test 2"
            };
            var updateProducerDTO = new UpdateProducerDTO {
                Id = producerId,
                Name = "Producer Test 2"
            };

            _producerRepositoryMock.Setup(x => x.Update(It.IsAny<backend.Models.Producer>())).ReturnsAsync(expectedProducer);
            _producerRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(producer);

            UpdateProducerUseCase updateProducerUseCase = new UpdateProducerUseCase(_producerRepositoryMock.Object);

            //Act
            var updatedProducer = await updateProducerUseCase.Execute(updateProducerDTO);

            //Assert
            Assert.NotNull(updatedProducer);
            Assert.NotEqual(expectedProducer.Name, producer.Name);
        }

        [Fact]
        [Trait("OP","Update")]
        public async Task Update_GivenNotExistantProducer_ThrowsException() {
            //Arrange
            _producerRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<backend.Models.Producer>(null));
            UpdateProducerUseCase updateProducerUseCase = new UpdateProducerUseCase(_producerRepositoryMock.Object);
            UpdateProducerDTO updateProducerDTO = new UpdateProducerDTO {
                Id = Guid.NewGuid(),
            };

            //Act
            async Task Act() {
                await updateProducerUseCase.Execute(updateProducerDTO);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async ()=> await Act());
            Assert.Equal("Produtor não existe", exception.Message);
        }
    }
}
