using backend.Contexts;
using backend.Product.DTOs;
using backend.Product.Repository;
using backend.Product.UseCases;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories.Product;

namespace Tests.UnitTests.UseCases
{
    public class CreateManyProductsUseCaseTest {

        private readonly Mock<IProductRepository> _productRepositoryMock;

        public CreateManyProductsUseCaseTest() {
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        [Trait("OP", "CreateMany")]
        public async Task SaveMany_GivenListOfCreateProductDTO_ReturnsListOfCreatedProducts() {

            //Arrange
            Guid producerId = Guid.NewGuid();

            var products = new List<backend.Models.Product> {
                new ProductFactory()
                    .WithProducerId(producerId)
                    .Build(),
                new ProductFactory()
                    .WithProducerId(producerId)
                    .WithName("Product 2")
                    .WithDescription("Description 2")
                    .Build()
            };

            _productRepositoryMock.Setup(x => x.SaveMany(It.IsAny<backend.Models.Product[]>())).ReturnsAsync(products);
            
            CreateManyProductsUseCase usecase = new CreateManyProductsUseCase(_productRepositoryMock.Object);
            ProductDTOFactory productFactory = new ProductDTOFactory();

            var productsDTO = new CreateProductDTO[]{
                productFactory.Build(),
                productFactory.WithName("Product 2")
                    .WithDescription("Description 2")
                    .Build()
            };
            //Act
            var createdProducts = await usecase.Execute( productsDTO );

            //Assert
            Assert.Equal(productsDTO.Length, createdProducts.Count());
        }

        [Fact]
        [Trait("OP", "CreateMany")]
        public async Task SaveMany_GivenListOfProductsDTOWithInvalidHarvestDate_ThrowsException() {
            //Arrange
            Guid producerId = Guid.NewGuid();

            CreateManyProductsUseCase usecase = new CreateManyProductsUseCase(_productRepositoryMock.Object);
            ProductDTOFactory productFactory = new ProductDTOFactory();

            var productsDTO = new CreateProductDTO[]{
                productFactory.WithHarvestDate("31/31/1231")
                    .Build(),
                productFactory.WithName("Product 2")
                    .WithDescription("Description 2")
                    .Build()
            };
            //Act
            async Task Act() {
                var createdProducts = await usecase.Execute(productsDTO);
            }
            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act());
            Assert.Equal("Formato inválido de data", exception.Message);
        }
    }
}
