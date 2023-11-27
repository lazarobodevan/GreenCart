using backend.Contexts;
using backend.DTOs.Producer;
using backend.Models;
using backend.Repositories;
using backend.UseCases.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories;

namespace Tests.UnitTests.UseCases.Producer {
    public class FindProducerByIdUseCaseTest : IAsyncLifetime {

        private DatabaseContext databaseContext;
        private ProducerFactory producerFactory = new ProducerFactory();

        public FindProducerByIdUseCaseTest() {
            databaseContext = DbContextFactory.GetDatabaseContext();
        }

        public Task DisposeAsync() {
            return databaseContext.Database.EnsureDeletedAsync();
        }

        public Task InitializeAsync() {
            return databaseContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task ShouldFindProducerByIdSuccessfully() {
            //Arrange
            ProducerRepository producerRepository = new ProducerRepository(databaseContext);
            CreateProducerUseCase createProducerUseCase = new CreateProducerUseCase(producerRepository);
            FindProducerByIdUseCase findProducerByIdUseCase = new FindProducerByIdUseCase(producerRepository);

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
        public async Task ShouldFailToFindProducerAndReturnNull() {
            
            //Arrange
            ProducerRepository producerRepository = new ProducerRepository(databaseContext);
            FindProducerByIdUseCase findProducerByIdUseCase = new FindProducerByIdUseCase(producerRepository);

            //Act
            var foundProducer = await findProducerByIdUseCase.Execute(Guid.NewGuid());

            //Assert
            Assert.Null(foundProducer);
        }
    }
}
