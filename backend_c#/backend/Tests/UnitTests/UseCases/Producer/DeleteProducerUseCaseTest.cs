using backend.Models;
using backend.Repositories;
using backend.UseCases.Producer;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases.Producer {
    public class DeleteProducerUseCaseTest {

        private Mock<IProducerRepository> _producerRepositoryMock;

        public DeleteProducerUseCaseTest() {
            _producerRepositoryMock = new Mock<IProducerRepository>();
        }

        [Fact]
        [Trait("OP", "Delete")]
        public async Task Delete_GivenProducer_ReturnsDeletedProducer() {
            //Arrange
            var producerId = Guid.NewGuid();
            var producer = new backend.Models.Producer {
                Id = producerId,
            };
            DeleteProducerUseCase deleteProducerUseCase = new DeleteProducerUseCase(_producerRepositoryMock.Object);

            _producerRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(new backend.Models.Producer {
                Id = producerId,
            });

            _producerRepositoryMock.Setup(x => x.Delete(It.IsAny<backend.Models.Producer>())).ReturnsAsync(new backend.Models.Producer {
                Id = producerId,
                DeletedAt = DateTime.Now
            });

            //Act
            var deletedProducer = await deleteProducerUseCase.Execute(producerId);

            //Assert
            Assert.NotNull(deletedProducer);
            Assert.NotEqual(DateTime.MinValue, deletedProducer.DeletedAt);
        }

        [Fact]
        [Trait("OP", "Delete")]
        public async Task Delete_GivenNotExistantProducer_ThrowsException() {
            //Arrange
            var producerId = Guid.NewGuid();
            DeleteProducerUseCase deleteProducerUseCase = new DeleteProducerUseCase(_producerRepositoryMock.Object);

            _producerRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<backend.Models.Producer?>(null));

            //Act
            async Task Act(Guid producerId) {
                var deletedProducer = await deleteProducerUseCase.Execute(producerId);
            }
            //Assert

            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act(producerId));
            Assert.Equal("Produtor não existe", exception.Message);
        }

    }
}
