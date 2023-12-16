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
using Tests.Factories;

namespace Tests.UnitTests.UseCases {
    public class GetProductByIdUseCaseTest: IAsyncLifetime {
        private static DbContextOptions<DatabaseContext> dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "DbTest").Options;

        DatabaseContext _databaseContext;

        public GetProductByIdUseCaseTest() {
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
        [Trait("OP", "FindById")]
        public async Task ShouldFindProductByIdSuccessfully() {

            //Arrange
            ProductRepository repository = new ProductRepository(_databaseContext);
            GetProductByIdUseCase getProductByIdUseCase = new GetProductByIdUseCase(repository);
            CreateProductUseCase createProductUseCase = new CreateProductUseCase(repository);

            ProductDTOFactory productFactory = new ProductDTOFactory();

            Guid producerId = Guid.NewGuid();

            var productDTO = productFactory.GetDefaultCreateProductDto(producerId).Build();
            //Act
            var createdProduct = await createProductUseCase.Execute(productDTO);
            var possibleProduct = await getProductByIdUseCase.Execute(createdProduct.Id);

            //Assert
            Assert.NotNull( possibleProduct );
        }
    }
}
