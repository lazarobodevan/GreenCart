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

namespace Tests.UnitTests.UseCases.Product {
    
    public class UpdateProductUseCaseTest {

        [Fact]
        public async Task UpdateExistantProductSuccessfully() {
            //Arrange
            var dbContext = DbContextFactory.GetDatabaseContext();
            ProductRepository repository = new ProductRepository(dbContext);
            UpdateProductUseCase updateProductUsecase = new UpdateProductUseCase(repository);
            CreateProductUseCase createProductUseCase = new CreateProductUseCase(repository);

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
                "22/11/2023",
                new Guid()
            );

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
