using backend.Contexts;
using backend.DTOs.Product;
using backend.Enums;
using backend.Repositories;
using backend.UseCases.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases {
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
        public async Task CreateManyProductsUseCaseSuccessfully() {
            
            //Arrange
            ProductRepository repository = new ProductRepository(_databaseContext);
            CreateManyProductsUseCase usecase = new CreateManyProductsUseCase(repository);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var productsDTO = new CreateProductDTO[]{
                new CreateProductDTO(
                    "Product",
                    "Description",
                    picture,
                    Category.VEGETABLE,
                    10.10,
                    Unit.UNIT,
                    10,
                    true,
                    new DateTime(),
                    producerId
                ),
                new CreateProductDTO(
                    "Product2",
                    "Description2",
                    picture,
                    Category.SWEET,
                    23.10,
                    Unit.LITER,
                    2,
                    true,
                    new DateTime(),
                    producerId
                ),
            };
            //Act
            var createdProducts = await usecase.Execute( productsDTO );

            //Assert
            Assert.Equal(productsDTO.Length, createdProducts.Count());
        }
    }
}
