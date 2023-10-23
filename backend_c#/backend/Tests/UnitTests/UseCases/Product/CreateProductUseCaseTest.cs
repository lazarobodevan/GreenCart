using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Repositories;
using backend.UseCases.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.UseCases {
    public class CreateProductUseCaseTest {
        [Fact]
        public async Task CreateProductUseCaseSuccessfully() {
            //Arrange
            var dbContext = DbContextFactory.GetDatabaseContext();
            ProductRepository repository = new ProductRepository(dbContext);
            CreateProductUseCase usecase = new CreateProductUseCase(repository);

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            var productDTO = new CreateProductDTO(
                "Product",
                "Description",
                picture,
                Category.VEGETABLE,
                10.10,
                Unit.UNIT,
                10,
                true,
                new DateTime(),
                new Guid()
            );
            //Act

            Product product =  await usecase.Execute(productDTO);
            //Assert
            Assert.NotNull(product);
        }
    }
}
