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

namespace Tests.UnitTests.Repositories {
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
        public async Task SaveProductSuccessfully() {
            
            var productRepository = new ProductRepository(_databaseContext);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            var product = new Product {
                Name = "Product",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = Guid.NewGuid(),
            };

            //Act
            var createdProduct = await productRepository.Save(product);

            //Assert

            Assert.NotNull( createdProduct );
            Assert.NotEqual(Guid.Empty, createdProduct.Id);
        }

        [Fact] 
        public async Task FindProductByIdSuccessfully() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            var product = new Product {
                Name = "Product",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = Guid.NewGuid(),
            };

            //Act
            var createdProduct = await productRepository.Save(product);
            var possibleProduct = await productRepository.FindById(createdProduct.Id);

            //Assert
            Assert.NotNull( possibleProduct );
        }

        [Fact]
        public async Task FindProductByIdFail() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            //Act
            var possibleProduct = await productRepository.FindById(Guid.NewGuid());

            //Assert
            Assert.Null( possibleProduct );

        }

        [Fact]
        public async Task SaveManyProductsSuccessfully() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var products = new Product[]{
                new Product {
                    Name = "Product",
                    Description = "Description",
                    Picture = picture,
                    Category = Category.VEGETABLE,
                    Price = 10.11,
                    Unit = Unit.LITER,
                    AvailableQuantity = 1,
                    IsOrganic = true,
                    HarvestDate = DateTime.Now,
                    ProducerId = producerId,
                },
                new Product {
                    Name = "Product2",
                    Description = "Description2",
                    Picture = picture,
                    Category = Category.SWEET,
                    Price = 40.43,
                    Unit = Unit.LITER,
                    AvailableQuantity = 14,
                    IsOrganic = false,
                    HarvestDate = DateTime.Now,
                    ProducerId = producerId,
                }

            };

            //Act
            var createdProducts = await productRepository.SaveMany(products);

            //Assert
            Assert.Equal(createdProducts.Count(), products.Length);
            
        }

        [Fact]
        public async Task UpdateProductSuccessfully() {
            //Arrange
            ProductRepository repository = new ProductRepository(_databaseContext);
            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            var product = new Product {
                Name = "Product",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = Guid.NewGuid(),
            };

            //Act
            var savedProduct = await repository.Save(product);
            savedProduct.Name = "Updated Product";
            savedProduct.IsOrganic = false;
            var updatedProduct = await repository.Update(savedProduct);

            //Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal(savedProduct.Id, updatedProduct.Id);
            Assert.Equal("Updated Product", updatedProduct.Name);
            Assert.False(updatedProduct.IsOrganic);
            Assert.NotEqual(updatedProduct.CreatedAt, updatedProduct.UpdatedAt);
        }

        [Fact]
        public async Task UpdateNotExistantProductFail() {
            //Arrange
            ProductRepository repository = new ProductRepository(_databaseContext);
            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            var product = new Product {
                Id = Guid.NewGuid(),
                Name = "Product",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = Guid.NewGuid(),
            };

            //Act
            var savedProduct = product;
            savedProduct.Name = "Updated Product";
            savedProduct.IsOrganic = false;

            async Task Act() {
                var updatedProduct = await repository.Update(savedProduct!);
            }

            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await Act());
        }

        [Fact]
        public async Task DeleteProductSuccessfully() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            var product = new Product {
                Name = "Product",
                Description = "Description",
                Picture = picture,
                Category = Category.VEGETABLE,
                Price = 10.11,
                Unit = Unit.LITER,
                AvailableQuantity = 1,
                IsOrganic = true,
                HarvestDate = DateTime.Now,
                ProducerId = Guid.NewGuid(),
            };

            //Act
            var createdProduct = await productRepository.Save(product);
            Assert.Null(createdProduct.DeletedAt);
            var deletedProduct = await productRepository.Delete(createdProduct.Id);

            //Assert
            
            Assert.NotNull(deletedProduct.DeletedAt);
        }

        [Fact]
        public async Task TryToDeleteNotExistantProductAndThrowAnError() {
            //Arrange
            var productRepository = new ProductRepository(_databaseContext);

            //Act
            async Task Act() {
                var deletedProduct = await productRepository.Delete(Guid.NewGuid());
            }

            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await Act());
        }
    }
}
