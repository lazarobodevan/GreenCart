using backend.Contexts;
using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.Repositories {
    public class ProducerRepositoryTest {
        private DatabaseContext _dbContext;

        public ProducerRepositoryTest() {
            _dbContext = DbContextFactory.GetDatabaseContext();
        }

        [Fact]
        public async Task CreateProducerSuccessfully() {
            //Arrange
            ProducerRepository repository = new ProducerRepository(this._dbContext);
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                Attended_Cities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                Where_to_Find = "Local de encontro"
            };

            //Act
            var createdProducer = await repository.Save(producer);

            //Assert
            Assert.NotNull(createdProducer);
        }

        [Fact]
        public async Task CreateProducerFail() {
            //Arrange
            ProducerRepository repository = new ProducerRepository(this._dbContext);
            
            // Missing mandatory field Name
            var producer = new Producer {
                
                Email = "test@test.com",
                Attended_Cities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                Where_to_Find = "Local de encontro"
            };

            //Act
            async Task Act() {
                var createdProducer = await repository.Save(producer);
            }

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async() => await Act());
        }

        [Fact]
        public async Task FindProducerByIdSuccessfully() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                Attended_Cities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                Where_to_Find = "Local de encontro"
            };

            //Act
            var createdProducer = await repository.Save(producer);
            var possibleProducer = await repository.FindById(createdProducer.Id);

            //Assert
            Assert.NotNull(possibleProducer);
        }

        [Fact]
        public async Task FindProducerByIdFail() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);

            //Act
            var possibleProducer = await repository.FindById(Guid.NewGuid());

            //Assert
            Assert.Null(possibleProducer);
        }

        [Fact]
        public async Task FindProducerByEmailSuccessfully() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                Attended_Cities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                Where_to_Find = "Local de encontro"
            };

            //Act
            var createdProducer = await repository.Save(producer);
            var possibleProducer = await repository.FindByEmail(createdProducer.Email);

            //Assert
            Assert.NotNull(possibleProducer);
            Assert.Equal(possibleProducer.Email, createdProducer.Email);
        }

        [Fact]
        public async Task FindProducerByEmailFail() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                Attended_Cities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                Where_to_Find = "Local de encontro"
            };

            var fakeEmail = "test@fake.com";

            //Act
            var createdProducer = await repository.Save(producer);
            var possibleProducer = await repository.FindByEmail(fakeEmail);

            //Assert
            Assert.Null(possibleProducer);
        }

        [Fact]
        public async Task GetProducerProductsSuccessfully() {
            //Arrange
            var productRepository = new ProductRepository(_dbContext);
            var producerRepository = new ProducerRepository(_dbContext);
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                Attended_Cities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                Where_to_Find = "Local de encontro"
            };

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var productDTO = new CreateProductDTO(
                "Product",
                "Description",
                picture,
                Category.VEGETABLE,
                10.10,
                Unit.UNIT,
                10,
                true,
                new DateTime(),
                new Guid()
            );
        }

    }
}
