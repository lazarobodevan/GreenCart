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
using Tests.Factories;
using Tests.Factories.Product;

namespace Tests.UnitTests.Repositories
{
    public class ProductRepositoryTest : IAsyncLifetime{

        private static DbContextOptions<DatabaseContext> dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "DbTest").Options;

        DatabaseContext _databaseContext;

        public ProductRepositoryTest() { 
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
        public async Task Save_GivenProduct_ReturnsCreatedProduct() {
            
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);
            var product = new ProductFactory().Build();

            //Act
            var createdProduct = await productRepository.Save(product);

            //Assert
            Assert.NotNull( createdProduct );
            Assert.NotEqual(Guid.Empty, createdProduct.Id);
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProduct_ReturnsFoundProduct() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            var product = new ProductFactory().Build();

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
        public async Task Create_GivenManyProducts_ReturnsCreatedProducts() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var products = new Product[]{
                new ProductFactory().Build(),
                new ProductFactory().Build()
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
            var product = new ProductFactory().Build();

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
            var product = new ProductFactory().Build();

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
