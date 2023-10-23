using backend.Contexts;
using backend.DTOs.Producer;
using backend.Models;
using backend.Repositories;
using backend.UseCases.Producer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases.Producer {
    public class CreateProducerUseCaseTest {

        private static DbContextOptions<DatabaseContext> dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "DbTest").Options;

        DatabaseContext _databaseContext;

        public CreateProducerUseCaseTest() {
            _databaseContext = new DatabaseContext(dbContextOptions);
        }


        [Fact]
        public async Task CreateProducerSuccessfully() {
            
            //Arrange
            ProducerRepository repository = new ProducerRepository(_databaseContext);
            CreateProducerUseCase usecase = new CreateProducerUseCase(repository);

            var producer = new CreateProducerDTO{
                Name = "Producer Test",
                Email = "test@test.com",
                Attended_Cities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro"
            };

            //Act
            var createdProducer = await usecase.Execute(producer);

            //Assert
            Assert.NotNull(createdProducer);
        }
    }
}
