using backend.Models;
using backend.Product.Repository;
using backend.Product.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories;

namespace Tests.UnitTests.UseCases
{
    public class CreateProductUseCaseTest {
        [Fact]
        [Trait("OP", "Create")]
        public async Task ShouldCreateProductUseCaseSuccessfully() {
            //Arrange
            var dbContext = DbContextFactory.GetDatabaseContext();
            ProductRepository repository = new ProductRepository(dbContext);
            CreateProductUseCase createProductUseCase = new CreateProductUseCase(repository);

            ProductDTOFactory productFactory = new ProductDTOFactory();

            var producerId = Guid.NewGuid();

            var productDTO = productFactory.GetDefaultCreateProductDto(producerId).Build();
            
            //Act
            backend.Models.Product product =  await createProductUseCase.Execute(productDTO);

            //Assert
            Assert.NotNull(product);
        }
    }
}
