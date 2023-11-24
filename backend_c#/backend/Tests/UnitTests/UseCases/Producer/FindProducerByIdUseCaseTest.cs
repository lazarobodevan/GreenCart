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

namespace Tests.UnitTests.UseCases.Producer {
    public class FindProducerByIdUseCaseTest : IAsyncLifetime {

        private DatabaseContext databaseContext;

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

            var producer = new CreateProducerDTO {
                Name = "Producer Test",
                Email = "test@test.com",
                AttendedCities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro"
            };

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
