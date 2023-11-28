using backend.Contexts;
using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//***** DO NOT REMOVE*****
[assembly: CollectionBehavior(DisableTestParallelization = false)]
//************************

namespace Tests.UnitTests.Repositories {

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
        public async Task FindByEmail_GivenNotExistantEmail_ReturnsNull() {
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
        [Trait("OP", "GetProducerProducts")]
        public async Task GetProducerProducts_GivenProducerId_ReturnProducerProducts() {
            /*
             * Producer 1 has 1 Product called Product1;
             * Producer 2 has 2 Products called Product2 and Product3
             * Should find only 1 product for Producer1 and 2 products for Producer2
            */

            //Arrange
            var producer1 = new Producer {
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
            var producer2 = new Producer {
                Name = "Producer Test2",
                Email = "test2@test.com",
                AttendedCities = "City2;City3",
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

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var product1 = new Product {
                Name = "Product1",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer1.Id,
            };
            var product2 = new Product {
                Name = "Product2",
                Description = "Description",
                Picture = picture,
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };
            var product3 = new Product {
                Name = "Product3",
                Description = "Description",
                Picture = picture,
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };

            var createdProduct1 = await _productRepository.Save(product1);

            var createdProducts2And3 = await _productRepository.SaveMany(new Product[] { product2, product3 });

            //Act
            var foundProductsFromProducer1 = _producerRepository.GetProducts(createdProducer1.Id);
            var foundProductsFromProducer2 = _producerRepository.GetProducts(createdProducer2.Id);

            var isFoundProductsFromProducer2ContainsProduct1 = foundProductsFromProducer2.Any(product => product.Name == product1.Name);
            var isFoundProductsFromProducer2ContainsProduct2 = foundProductsFromProducer2.Any(product => product.Name == product2.Name);
            var isFoundProductsFromProducer2ContainsProduct3 = foundProductsFromProducer2.Any(product => product.Name == product3.Name);

            //Assert
            Assert.Single(foundProductsFromProducer1);
            Assert.Equal(foundProductsFromProducer1.First().Name, product1.Name);

            Assert.Equal(2, foundProductsFromProducer2.Count());
            Assert.False(isFoundProductsFromProducer2ContainsProduct1);
            Assert.True(isFoundProductsFromProducer2ContainsProduct2);
            Assert.True(isFoundProductsFromProducer2ContainsProduct3);
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public void GetProducerProducts_GivenNotExistantProducerId_ReturnsEmptyList() {
            //Arrange
            var producerId = Guid.NewGuid();

            //Act
            var possibleProduct = _producerRepository.GetProducts(Guid.NewGuid());

            //Assert
            Assert.Empty(possibleProduct);
        }

        [Fact]
        [Trait("OP", "GetNearProducers")]
        public async Task GetNearProducers_GivenCity1_ReturnsOneProducerFromCity1() {
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

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var product1 = new Product {
                Name = "Product1",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer1.Id,
            };
            var product2 = new Product {
                Name = "Product2",
                Description = "Description",
                Picture = picture,
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };
            var product3 = new Product {
                Name = "Product3",
                Description = "Description",
                Picture = picture,
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };

            var createdProduct1 = await _productRepository.Save(product1);

            var createdProducts2And3 = await _productRepository.SaveMany(new Product[] { product2, product3 });

            //Act
            var producersFromCity1 = _producerRepository.GetNearProducers("city1").ToList();

            var isProducersFromCity1ContainsProducer2 = producersFromCity1.Any(producer => producer.Name.Contains(producer2.Name));

            //Assert
            Assert.False(isProducersFromCity1ContainsProducer2);
            Assert.Single(producersFromCity1);
            Assert.NotNull(producersFromCity1.First().Products);
        }

        [Fact]
        [Trait("OP", "GetNearProducers")]
        public async Task GetNearProducers_GivenCity2_ReturnsTwoProducersFromCity2() {
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

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var product1 = new Product {
                Name = "Product1",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer1.Id,
            };
            var product2 = new Product {
                Name = "Product2",
                Description = "Description",
                Picture = picture,
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };
            var product3 = new Product {
                Name = "Product3",
                Description = "Description",
                Picture = picture,
                Category = Category.GRAIN,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = createdProducer2.Id,
            };

            var createdProduct1 = await _productRepository.Save(product1);

            var createdProducts2And3 = await _productRepository.SaveMany(new Product[] { product2, product3 });

            //Act
            var producersFromCity2 = _producerRepository.GetNearProducers("city2").ToList();

            var isProducersFromCity2ContainsProducer1 = producersFromCity2.Any(producer => producer.Name.Contains(producer1.Name));
            var isProducersFromCity2ContainsProducer2 = producersFromCity2.Any(producer => producer.Name.Contains(producer2.Name));

            //Assert
            Assert.True(isProducersFromCity2ContainsProducer1);
            Assert.True(isProducersFromCity2ContainsProducer2);
            Assert.Equal(2,producersFromCity2.Count());
            Assert.NotNull(producersFromCity2.ToList().ElementAt(0).Products);
            Assert.NotNull(producersFromCity2.ToList().ElementAt(1).Products);
        }

    }
}
