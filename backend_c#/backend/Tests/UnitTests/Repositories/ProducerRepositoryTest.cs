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

//***** DO NOT REMOVE*****
[assembly: CollectionBehavior(DisableTestParallelization = true)]
//************************

namespace Tests.UnitTests.Repositories {

    public class ProducerRepositoryTest : IAsyncLifetime {
        private DatabaseContext _dbContext;

        public ProducerRepositoryTest() {
            _dbContext = DbContextFactory.GetDatabaseContext();
        }

        public async Task InitializeAsync() {
            await this._dbContext.Database.EnsureDeletedAsync();
        }

        public async Task DisposeAsync() {
            await this._dbContext.Database.EnsureDeletedAsync();
        }


        [Fact]
        [Trait("OP","Create")]
        public async Task ShouldCreateProducerSuccessfully() {
            //Arrange
            ProducerRepository repository = new ProducerRepository(this._dbContext);
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
            var createdProducer = await repository.Save(producer);

            //Assert
            Assert.NotNull(createdProducer);
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task ShouldCreateProducerFail() {
            //Arrange
            ProducerRepository repository = new ProducerRepository(this._dbContext);
            
            // Missing mandatory field Name
            var producer = new Producer {
                
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
            async Task Act() {
                var createdProducer = await repository.Save(producer);
            }

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async() => await Act());
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task ShouldFindProducerByIdSuccessfully() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);
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
            var createdProducer = await repository.Save(producer);
            var possibleProducer = await repository.FindById(createdProducer.Id);

            //Assert
            Assert.NotNull(possibleProducer);
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task ShouldFindProducerByIdFail() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);

            //Act
            var possibleProducer = await repository.FindById(Guid.NewGuid());

            //Assert
            Assert.Null(possibleProducer);
        }

        [Fact]
        [Trait("OP", "FindByEmail")]
        public async Task ShouldFindProducerByEmailSuccessfully() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);
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
            var createdProducer = await repository.Save(producer);
            var possibleProducer = await repository.FindByEmail(createdProducer.Email);

            //Assert
            Assert.NotNull(possibleProducer);
            Assert.Equal(possibleProducer.Email, createdProducer.Email);
        }

        [Fact]
        [Trait("OP", "FindByEmail")]
        public async Task ShouldFindProducerByEmailFail() {
            //Arrange
            var repository = new ProducerRepository(_dbContext);
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
            var createdProducer = await repository.Save(producer);
            var possibleProducer = await repository.FindByEmail(fakeEmail);

            //Assert
            Assert.Null(possibleProducer);
        }

        /*
         * Producer 1 has 1 Product called Product1;
         * Producer 2 has 2 Products called Product2 and Product3
         * Should find only 1 product for Producer1 and 2 products for Producer2
        */
        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task ShouldGetProducerProductsSuccessfully() {
            //Arrange
            var productRepository = new ProductRepository(_dbContext);
            var producerRepository = new ProducerRepository(_dbContext);
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

            var createdProducer1 = await producerRepository.Save(producer1);
            var createdProducer2 = await producerRepository.Save(producer2);

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

            var createdProduct1 = await productRepository.Save(product1);

            var createdProducts2And3 = await productRepository.SaveMany(new Product[] { product2, product3 });

            //Act
            var foundProductsFromProducer1 = producerRepository.GetProducts(createdProducer1.Id);
            var foundProductsFromProducer2 = producerRepository.GetProducts(createdProducer2.Id);

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
        public void ShouldGetProducerProductsFail() {
            //Arrange
            var producerRepository = new ProducerRepository(_dbContext);

            //Act
            var possibleProduct = producerRepository.GetProducts(Guid.NewGuid());

            //Assert
            Assert.Empty(possibleProduct);
        }

        [Fact]
        [Trait("OP", "GetNearProducers")]
        public async Task ShouldGetNearProducersFromCity1AndReturnOneProducerSuccessfully() {
            //Arrange
            var productRepository = new ProductRepository(_dbContext);
            var producerRepository = new ProducerRepository(_dbContext);
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

            var createdProducer1 = await producerRepository.Save(producer1);
            var createdProducer2 = await producerRepository.Save(producer2);

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

            var createdProduct1 = await productRepository.Save(product1);

            var createdProducts2And3 = await productRepository.SaveMany(new Product[] { product2, product3 });

            //Act
            var producersFromCity1 = producerRepository.GetNearProducers("city1").ToList();

            var isProducersFromCity1ContainsProducer2 = producersFromCity1.Any(producer => producer.Name.Contains(producer2.Name));

            //Assert
            Assert.False(isProducersFromCity1ContainsProducer2);
            Assert.Single(producersFromCity1);
            Assert.NotNull(producersFromCity1.First().Products);
        }

        [Fact]
        [Trait("OP", "GetNearProducers")]
        public async Task ShouldGetNearProducersFromCity2AndShouldReturnTwoProducersSuccessfully() {
            //Arrange
            var productRepository = new ProductRepository(_dbContext);
            var producerRepository = new ProducerRepository(_dbContext);
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

            var createdProducer1 = await producerRepository.Save(producer1);
            var createdProducer2 = await producerRepository.Save(producer2);

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

            var createdProduct1 = await productRepository.Save(product1);

            var createdProducts2And3 = await productRepository.SaveMany(new Product[] { product2, product3 });

            //Act
            var producersFromCity2 = producerRepository.GetNearProducers("city2").ToList();

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
