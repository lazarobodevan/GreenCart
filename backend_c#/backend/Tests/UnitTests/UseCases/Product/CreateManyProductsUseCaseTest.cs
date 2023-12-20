using backend.Contexts;
using backend.Product.DTOs;
using backend.Product.Repository;
using backend.Product.UseCases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories.Product;

namespace Tests.UnitTests.UseCases
{
    public class CreateManyProductsUseCaseTest: IAsyncLifetime {
        private static DbContextOptions<DatabaseContext> dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "DbTest").Options;

        DatabaseContext _databaseContext;

        public CreateManyProductsUseCaseTest() {
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
        [Trait("OP", "CreateMany")]
        public async Task ShouldCreateManyProductsUseCaseSuccessfully() {
            
            //Arrange
            ProductRepository repository = new ProductRepository(_databaseContext);
            CreateManyProductsUseCase usecase = new CreateManyProductsUseCase(repository);
            ProductDTOFactory productFactory = new ProductDTOFactory();

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var productsDTO = new CreateProductDTO[]{
                productFactory.GetDefaultCreateProductDto(producerId).Build(),
                productFactory.GetDefaultCreateProductDto(producerId)
                    .WithName("Product 2")
                    .WithDescription("Description 2")
                    .Build()
            };
            //Act
            var createdProducts = await usecase.Execute( productsDTO );

            //Assert
            Assert.Equal(productsDTO.Length, createdProducts.Count());
        }
    }
}
