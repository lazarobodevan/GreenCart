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

        private static DbContextOptions<DatabaseContext> dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "BookDbTest").Options;

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
        public async Task findProductByIdSuccessfully() {
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
        public async Task FindProductByIdNegative() {
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

    }
}
