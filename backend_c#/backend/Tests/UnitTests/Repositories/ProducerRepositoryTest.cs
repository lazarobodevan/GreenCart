using backend.Contexts;
using backend.Models;
using backend.Picture.DTOs;
using backend.Producer.Repository;
using backend.Product.Enums;
using backend.Product.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Shared.Factories.Picture;

//***** DO NOT REMOVE*****
[assembly: CollectionBehavior(DisableTestParallelization = false)]
//************************

namespace Tests.UnitTests.Repositories
{

    public class ProducerRepositoryTest {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _dbContext;
        private readonly IProducerRepository _producerRepository;
        private readonly IProductRepository _productRepository;


        public ProducerRepositoryTest() {
            _options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("test_database").Options;
            _dbContext = new DatabaseContext(_options);
            _dbContext.Database.EnsureDeleted();
            _producerRepository = new ProducerRepository(_dbContext);
            this._productRepository = new ProductRepository(_dbContext);
        }


        [Fact]
        [Trait("OP","Create")]
        public async Task Save_GivenProducerEntity_ReturnsCreatedProducerEntity() {
            
            //Arrange
            var producer = new Producer {
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
            var createdProducer = await _producerRepository.Save(producer);

            //Assert
            Assert.NotNull(createdProducer);
            Assert.IsType<Producer>(createdProducer);
            Assert.False(Guid.Empty.Equals(createdProducer.Id));
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenInvalidProducerEntity_ThrowsException() {
            //Arrange
            // Missing fields
            var producer = new Producer {
               
            };

            //Act
            async Task Act() {
                var createdProducer = await _producerRepository.Save(producer);
            }

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async() => await Act());
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProducerId_ReturnsProducerEntity() {
            //Arrange
            var producer = new Producer {
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
            var createdProducer = await _producerRepository.Save(producer);
            var possibleProducer = await _producerRepository.FindById(createdProducer.Id);

            //Assert
            Assert.NotNull(possibleProducer);
            Assert.False(Guid.Empty.Equals(createdProducer.Id));
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenInvalidProducerId_ReturnsNull() {
            //Arrange
            var producerId = Guid.NewGuid();

            //Act
            var possibleProducer = await _producerRepository.FindById(producerId);

            //Assert
            Assert.Null(possibleProducer);
        }

        [Fact]
        [Trait("OP", "FindByEmail")]
        public async Task FindByEmail_GivenEmail_ReturnsProducerEntity() {
            //Arrange
            var producer = new Producer {
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
            var createdProducer = await _producerRepository.Save(producer);
            var possibleProducer = await _producerRepository.FindByEmail(createdProducer.Email);

            //Assert
            Assert.NotNull(possibleProducer);
            Assert.Equal(possibleProducer.Email, createdProducer.Email);
        }

        [Fact]
        [Trait("OP", "FindByEmail")]
        public async Task FindByEmail_GivenNotExistentEmail_ReturnsNull() {
            //Arrange
            var producer = new Producer {
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

            var fakeEmail = "test@fake.com";

            //Act
            var createdProducer = await _producerRepository.Save(producer);
            var possibleProducer = await _producerRepository.FindByEmail(fakeEmail);

            //Assert
            Assert.Null(possibleProducer);
        }

        [Fact]
        [Trait("OP", "FindNearProducers")]
        public async Task FindNearProducers_GivenCity1_ReturnsOneProducerFromCity1() {
            //Arrange
            var producer1 = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                AttendedCities = "CITY1;CITY2;CITY3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro"
            };
            var producer2 = new Producer {
                Name = "Producer Test2",
                Email = "test2@test.com",
                AttendedCities = "CITY2;CITY3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-12",
                OriginCity = "City3",
                Password = "123",
                Telephone = "(31) 99999-9990",
                WhereToFind = "Local de encontro2"
            };

            var createdProducer1 = await _producerRepository.Save(producer1);
            var createdProducer2 = await _producerRepository.Save(producer2);

            var product1Id = Guid.NewGuid();
            var product2Id = Guid.NewGuid();
            var product3Id = Guid.NewGuid();

            var product1 = new Product {
                Id = product1Id,
                Name = "Product1",
                Description = "Description",
                Pictures = new List<Picture>() { new PictureFactory().WithProductId(product1Id).Build()},
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer1.Id,
            };
            var product2 = new Product {
                Id = product2Id,
                Name = "Product2",
                Description = "Description",
                Pictures = new List<Picture>() { new PictureFactory().WithProductId(product2Id).Build() },
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };
            var product3 = new Product {
                Id = product3Id,
                Name = "Product3",
                Description = "Description",
                Pictures = new List<Picture>() { new PictureFactory().WithProductId(product3Id).Build() },
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };

            var createdProduct1 = await _productRepository.Save(product1, new List<CreatePictureDTO>());

            var createdProducts2And3 = await _productRepository.SaveMany(new List<Product>() { product2, product3 });

            //Act
            var producersFromCity1 = _producerRepository.FindNearProducers("city1").ToList();

            var isProducersFromCity1ContainsProducer2 = producersFromCity1.Any(producer => producer.Name.Contains(producer2.Name));

            //Assert
            Assert.False(isProducersFromCity1ContainsProducer2);
            Assert.Single(producersFromCity1);
            Assert.NotNull(producersFromCity1.First().Products);
        }

        [Fact]
        [Trait("OP", "FindNearProducers")]
        public async Task FindNearProducers_GivenCity2_ReturnsTwoProducersFromCity2() {
            //Arrange
            var producer1 = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                AttendedCities = "CITY1;CITY2;CITY3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro"
            };
            var producer2 = new Producer {
                Name = "Producer Test2",
                Email = "test2@test.com",
                AttendedCities = "CITY2;CITY3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-12",
                OriginCity = "City3",
                Password = "123",
                Telephone = "(31) 99999-9990",
                WhereToFind = "Local de encontro2"
            };

            var createdProducer1 = await _producerRepository.Save(producer1);
            var createdProducer2 = await _producerRepository.Save(producer2);

            var product1Id = Guid.NewGuid();
            var product2Id = Guid.NewGuid();
            var product3Id = Guid.NewGuid();

            var product1 = new Product {
                Id = product1Id,
                Name = "Product1",
                Description = "Description",
                Pictures = new List<Picture>() { new PictureFactory().WithProductId(product1Id).Build() },
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer1.Id,
            };
            var product2 = new Product {
                Id= product2Id,
                Name = "Product2",
                Description = "Description",
                Pictures = new List<Picture>() { new PictureFactory().WithProductId(product2Id).Build() },
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };
            var product3 = new Product {
                Id= product3Id,
                Name = "Product3",
                Description = "Description",
                Pictures = new List<Picture>() { new PictureFactory().WithProductId(product1Id).Build() },
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };

            var createdProduct1 = await _productRepository.Save(product1, new List<CreatePictureDTO>());

            var createdProducts2And3 = await _productRepository.SaveMany(new List<Product>() { product2, product3 });

            //Act
            var producersFromCity2 = _producerRepository.FindNearProducers("city2").ToList();

            var isProducersFromCity2ContainsProducer1 = producersFromCity2.Any(producer => producer.Name.Contains(producer1.Name));
            var isProducersFromCity2ContainsProducer2 = producersFromCity2.Any(producer => producer.Name.Contains(producer2.Name));

            //Assert
            Assert.True(isProducersFromCity2ContainsProducer1);
            Assert.True(isProducersFromCity2ContainsProducer2);
            Assert.Equal(2,producersFromCity2.Count());
            Assert.NotNull(producersFromCity2.ToList().ElementAt(0).Products);
            Assert.NotNull(producersFromCity2.ToList().ElementAt(1).Products);
        }

        [Fact]
        [Trait("OP", "Update")]
        public async Task Update_GivenProducer_ReturnsUpdatedProducer() {
            //Arrange
            var producer = new Producer {
                Id = Guid.NewGuid(),
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
            var createdProducer = await _producerRepository.Save(new backend.Models.Producer (producer));
            
            _dbContext.ChangeTracker.Clear();

            producer.Id = createdProducer.Id;
            producer.Name = "Test";
            producer.Telephone = "(31) 99999-9991";
            producer.WhereToFind = "Local";
            producer.Email = "test2@test.com";
            producer.AttendedCities = "City1";
            producer.CPF = "011.111.111-11";
            producer.OriginCity = "City2";
            producer.Password = "321";

            var updatedProducer = await _producerRepository.Update(producer);

            //Assert
            Assert.NotNull(createdProducer);
            Assert.IsType<Producer>(createdProducer);
            
            Assert.Equal(updatedProducer.Id, createdProducer.Id);
            Assert.NotEqual(DateTime.MinValue, updatedProducer.UpdatedAt);
            Assert.NotEqual(updatedProducer.Name, createdProducer.Name);
            Assert.NotEqual(updatedProducer.Telephone, createdProducer.Telephone);
            Assert.NotEqual(updatedProducer.WhereToFind, createdProducer.WhereToFind);
            Assert.NotEqual(updatedProducer.Email, createdProducer.Email);
            Assert.NotEqual(updatedProducer.CPF, createdProducer.CPF);
            Assert.NotEqual(updatedProducer.OriginCity, createdProducer.OriginCity);
            Assert.NotEqual(updatedProducer.Password, createdProducer.Password);
        }

        [Fact]
        [Trait("OP", "Delete")]
        public async Task Delete_GivenProducer_ReturnsDeletedProducer() {
            //Arrange
            var producer = new Producer {
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
            var createdProducer = await _producerRepository.Save(producer);
            Assert.NotNull(createdProducer);

            var deletedProducer = await _producerRepository.Delete(createdProducer);

            //Assert
            Assert.NotNull(deletedProducer);
            Assert.NotEqual(DateTime.MinValue, createdProducer.DeletedAt) ;
        }

        [Fact]
        [Trait("OP","Delete")]
        public async Task Delete_GivenNotExistentProducer_ThrowsException() {
            //Arrange
            var producerId = Guid.NewGuid();
            var producer = new backend.Models.Producer {
                Id = producerId
            };

            //Act
            async Task Act(backend.Models.Producer producer) {
                await _producerRepository.Delete(producer);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(async () => await Act(producer));
        }
    }
}
