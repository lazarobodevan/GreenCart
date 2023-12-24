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
using backend.Product.Exceptions;
using EntityFramework.Exceptions.Common;
using Tests.Factories;
using Tests.Factories.Product;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            var createdProduct = await productRepository.Save(product);

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
                var producer = await productRepository.Save(productEntity);
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
                var producer = await productRepository.Save(productEntity);
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
            var createdProduct = await productRepository.Save(product);
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
        [Trait("OP", "Create")]
        public async Task Save_GivenManyProducts_ReturnsCreatedProducts() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var products = new Product[]{
                new Factories.Product.ProductFactory().Build(),
                new Factories.Product.ProductFactory().Build()
            };

            //Act
            var createdProducts = await productRepository.SaveMany(products);

            //Assert
            Assert.Equal(createdProducts.Count(), products.Length);
            
        }

        [Fact]
        [Trait("OP", "Update")]
        public async Task Update_GivenProduct_ReturnsUpdatedProduct() {
            //Arrange
            ProductRepository repository = new ProductRepository(_databaseContext);
            var product = new Factories.Product.ProductFactory().Build();

            //Act
            var savedProduct = await repository.Save(product);
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
            var createdProduct = await productRepository.Save(product);
            Assert.Null(createdProduct.DeletedAt);
            var deletedProduct = await productRepository.Delete(createdProduct);

            //Assert
            Assert.NotNull(deletedProduct);
            Assert.NotEqual(DateTime.MinValue,deletedProduct.DeletedAt);
        }

    }
}
