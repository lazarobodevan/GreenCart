using backend.DTOs.Product;
using backend.Enums;
using backend.Repositories;
using backend.UseCases.Product;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories;

namespace Tests.UnitTests.UseCases.Product {
    
    public class UpdateProductUseCaseTest {

        [Fact]
        [Trait("OP", "Update")]
        public async Task ShouldUpdateExistantProductSuccessfully() {
            //Arrange
            var dbContext = DbContextFactory.GetDatabaseContext();
            ProductRepository repository = new ProductRepository(dbContext);
            UpdateProductUseCase updateProductUsecase = new UpdateProductUseCase(repository);
            CreateProductUseCase createProductUseCase = new CreateProductUseCase(repository);

            ProductFactory productFactory = new ProductFactory();

            Guid producerId = Guid.NewGuid();

            var productDTO = productFactory.GetDefaultCreateProductDto(producerId).Build();


            //Act
            var createdProduct = await createProductUseCase.Execute(productDTO);

            createdProduct.Name= "Updated Product";
            createdProduct.Category = Category.GRAIN;

            var updatedProduct = await updateProductUsecase.Execute(createdProduct);

            //Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal(createdProduct.Name, updatedProduct.Name);
            Assert.Equal(createdProduct.Category, updatedProduct.Category);

        }

    }
}
