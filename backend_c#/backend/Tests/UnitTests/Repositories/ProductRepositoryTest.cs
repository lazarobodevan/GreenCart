using backend.Contexts;
using backend.Models;
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
using backend.Producer.Repository;
using EntityFramework.Exceptions.Common;
using Tests.Factories;
using Tests.Factories.Product;
using Microsoft.EntityFrameworkCore.Infrastructure;
using backend.Product.Exceptions;
using backend.Product.DTOs;
using backend.Picture.DTOs;

namespace Tests.UnitTests.Repositories
{
    public class ProductRepositoryTest : IAsyncLifetime{

        private DbContextOptions<DatabaseContext> _dbContextOptions;

        DatabaseContext _databaseContext;

        public ProductRepositoryTest() {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "DbTest").Options;
            _databaseContext = new DatabaseContext(_dbContextOptions);
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
        public async Task Save_GivenProduct_ReturnsCreatedProduct() {
            
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);
            var product = new Factories.Product.ProductFactory().Build();

            //Act
            var createdProduct = await productRepository.Save(product, new List<CreatePictureDTO>());

            //Assert
            Assert.NotNull( createdProduct );
            Assert.NotEqual(Guid.Empty, createdProduct.Id);
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenNotExistentProducerId_ThrowsProducerDoesNotExistException(){
            //Arrange
            var _dbContext = new Mock<DatabaseContext>(_dbContextOptions);
            var productRepository = new ProductRepository(_dbContext.Object);
            var productEntity = new ProductFactory().Build();

            _dbContext.Setup(x => x.Products.AddAsync(productEntity, It.IsAny<CancellationToken>()))
                .Throws(new ReferenceConstraintException());

            //Act
            async Task Act(){
                var producer = await productRepository.Save(productEntity, new List<CreatePictureDTO>());
            }

            //Assert
            var exception = await Assert.ThrowsAsync<ProducerDoesNotExistException>(async () => await Act());
            Assert.Equal("Produtor não existe", exception.Message);
            Assert.IsType<ProducerDoesNotExistException>(exception);
        }

        [Fact]
        public async Task Save_GivenUnexpectedException_ThrowsException() {
            //Arrange
            var _dbContext = new Mock<DatabaseContext>(_dbContextOptions);
            var productRepository = new ProductRepository(_dbContext.Object);
            var productEntity = new ProductFactory().Build();

            _dbContext.Setup(x => x.Products.AddAsync(productEntity, It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            //Act
            async Task Act() {
                var producer = await productRepository.Save(productEntity, new List<CreatePictureDTO>());
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act());
            Assert.Equal("Erro inesperado ao salvar no banco de dados", exception.Message);
            Assert.IsType<Exception>(exception);
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProduct_ReturnsFoundProduct() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            var product = new Factories.Product.ProductFactory().Build();

            //Act
            var createdProduct = await productRepository.Save(product, new List<CreatePictureDTO>());
            var possibleProduct = await productRepository.FindById(createdProduct.Id);

            //Assert
            Assert.NotNull( possibleProduct );
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProductId_ReturnsNull() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            //Act
            var possibleProduct = await productRepository.FindById(Guid.NewGuid());

            //Assert
            Assert.Null( possibleProduct );

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
            var _productRepository = new ProductRepository(_databaseContext);
            var _producerRepository = new ProducerRepository(_databaseContext);

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

            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var productId3 = Guid.NewGuid();

            var picture1 = new List<Picture>() {
                new Picture() {
                    Key = Guid.NewGuid(),
                    Position = 0,
                    ProductId = productId1,
                }
            };
            var picture2 = new List<Picture>() {
                new Picture(){
                    Key = Guid.NewGuid(),
                    Position = 0,
                    ProductId = productId2,
                }
            };
            var picture3 = new List<Picture>() {
                new Picture() {
                    Key = Guid.NewGuid(),
                    Position = 0,
                    ProductId = productId3,
                }
            };
            var product1 = new Product {
                Name = "Product1",
                Description = "Description",
                Pictures = picture1,
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
                Pictures = picture2,
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
                Pictures = picture3,
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
            var foundProductsFromProducer1 = _productRepository.GetProducerProducts(createdProducer1.Id, 0, 2);
            var foundProductsFromProducer2 = _productRepository.GetProducerProducts(createdProducer2.Id, 0, 2);

            var isFoundProductsFromProducer2ContainsProduct1 = foundProductsFromProducer2.Products.Any(product => product.Name == product1.Name);
            var isFoundProductsFromProducer2ContainsProduct2 = foundProductsFromProducer2.Products.Any(product => product.Name == product2.Name);
            var isFoundProductsFromProducer2ContainsProduct3 = foundProductsFromProducer2.Products.Any(product => product.Name == product3.Name);

            //Assert
            Assert.Single(foundProductsFromProducer1.Products);
            Assert.Equal(foundProductsFromProducer1.Products.First().Name, product1.Name);

            Assert.Equal(2, foundProductsFromProducer2.Products.Count());
            Assert.False(isFoundProductsFromProducer2ContainsProduct1);
            Assert.True(isFoundProductsFromProducer2ContainsProduct2);
            Assert.True(isFoundProductsFromProducer2ContainsProduct3);
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public void GetProducerProducts_GivenNotExistentProducerId_ThrowsProducerDoesNotExistException() {
            //Arrange
            var producerId = Guid.NewGuid();
            var _productRepository = new ProductRepository(_databaseContext);

            //Act
            void Act() {
                var product = _productRepository.GetProducerProducts(Guid.NewGuid(), 0, 2);
            }
            //Assert
            Assert.Throws<ProducerDoesNotExistException>(() => Act());
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenManyProducts_ReturnsCreatedProducts() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var products = new List<Product>(){
                new Factories.Product.ProductFactory().Build(),
                new Factories.Product.ProductFactory().Build()
            };

            //Act
            var createdProducts = await productRepository.SaveMany(products);

            //Assert
            Assert.Equal(createdProducts.Count(), products.Count());
            
        }

        [Fact]
        [Trait("OP", "Update")]
        public async Task Update_GivenProduct_ReturnsUpdatedProduct() {
            //Arrange
            ProductRepository repository = new ProductRepository(_databaseContext);
            var product = new Factories.Product.ProductFactory().Build();

            //Act
            var savedProduct = await repository.Save(product, new List<CreatePictureDTO>());
            savedProduct.Name = "Updated Product";
            savedProduct.IsOrganic = false;
            savedProduct.UpdatedAt = DateTime.Now;

            var updatedProduct = repository.Update(savedProduct);

            //Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal(savedProduct.Id, updatedProduct.Id);
            Assert.Equal("Updated Product", updatedProduct.Name);
            Assert.False(updatedProduct.IsOrganic);
            Assert.NotEqual(updatedProduct.CreatedAt, updatedProduct.UpdatedAt);
        }

        [Fact]
        [Trait("OP", "Delete")]
        public async Task Delete_GivenProduct_ReturnsDeletedProduct() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);
            var product = new Factories.Product.ProductFactory().Build();

            //Act
            var createdProduct = await productRepository.Save(product, new List<CreatePictureDTO>());
            Assert.Null(createdProduct.DeletedAt);
            var deletedProduct = await productRepository.Delete(createdProduct);

            //Assert
            Assert.NotNull(deletedProduct);
            Assert.NotEqual(DateTime.MinValue,deletedProduct.DeletedAt);
        }

    }
}
