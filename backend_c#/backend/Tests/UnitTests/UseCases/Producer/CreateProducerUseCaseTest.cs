using backend.Contexts;
using backend.DTOs.Producer;
using backend.Models;
using backend.Repositories;
using backend.UseCases.Producer;
using backend.Utils.Errors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories;

namespace Tests.UnitTests.UseCases.Producer {

    public class CreateProducerUseCaseTest: IAsyncLifetime {

        private static DbContextOptions<DatabaseContext> dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "DbTest").Options;
        private ProducerFactory producerFactory = new ProducerFactory();

        DatabaseContext _databaseContext;

        public CreateProducerUseCaseTest() {
            _databaseContext = new DatabaseContext(dbContextOptions);
        }

        public Task DisposeAsync() {
            //Reset database
            return _databaseContext.Database.EnsureDeletedAsync();

        }

        public Task InitializeAsync() {
            //throw new NotImplementedException();
            return _databaseContext.Database.EnsureDeletedAsync();
        }


        [Fact]
        [Trait("OP", "Create")]
        public async Task ShouldCreateProducerSuccessfully() {
            
            //Arrange
            ProducerRepository repository = new ProducerRepository(_databaseContext);
            CreateProducerUseCase usecase = new CreateProducerUseCase(repository);

            var producer = producerFactory.GetDefaultCreateProducerDto();

            //Act
            var createdProducer = await usecase.Execute(producer);

            //Assert
            Assert.NotNull(createdProducer);
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task ShouldThrowErrorWhenCreatingProducerWithAlreadyExistantEmail() {
            //Arrange
            ProducerRepository repository = new ProducerRepository(_databaseContext);
            CreateProducerUseCase usecase = new CreateProducerUseCase(repository);

            var producer = producerFactory.GetDefaultCreateProducerDto();

            //Act
            async Task Act(CreateProducerDTO producer) {
                var createdProducer1 = await usecase.Execute(producer);
                var createdProducer2 = await usecase.Execute(producer);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act(producer));
            Assert.Equal("Usuário já cadastrado", exception.Message);
        }
    }
}
