using Moq;
using backend.Product.Repository;
using backend.Product.UseCases;
using backend.Product.DTOs;

namespace Tests.UnitTests.UseCases.Product
{

    public class UpdateProductUseCaseTest {

        private readonly Mock<IProductRepository> _productRepositoryMock;

        public UpdateProductUseCaseTest() {
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        [Trait("OP", "Update")]
        public async Task Update_GivenProduct_ReturnsUpdatedProduct() {
            //Arrange
            var productId = Guid.NewGuid();
            var product = new Factories.Product.ProductFactory()
                .WithId(productId)
                .Build();

            var updatedProductDTO = new UpdateProductDTO {
                Id = productId,
                Name = "Updated product"
            };
            var updatedProductEntity = new Factories.Product.ProductFactory()
                .WithId(productId)
                .WithName("Updated Product")
                .Build();

            _productRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(product);
            _productRepositoryMock.Setup(x => x.Update(It.IsAny<backend.Models.Product>())).Returns(updatedProductEntity);

            UpdateProductUseCase updateProductUsecase = new UpdateProductUseCase(_productRepositoryMock.Object);

            //Act
            var updatedProductResult = await updateProductUsecase.Execute(updatedProductDTO);

            //Assert
            Assert.NotNull(updatedProductResult);
            Assert.NotEqual(updatedProductEntity.Name, product.Name);
            Assert.Equal(updatedProductEntity.Name, updatedProductResult.Name);

        }

        [Fact]
        [Trait("OP", "Update")]
        public async Task Update_GivenNotExistantProduct_ThrowsException() {
            //Arrange
            _productRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<backend.Models.Product?>(null));

            UpdateProductUseCase updateProductUseCase = new UpdateProductUseCase(_productRepositoryMock.Object);

            UpdateProductDTO updateProductDTO = new UpdateProductDTO {
                Id = Guid.NewGuid(),
                Name = "Product"
            };

            //Act
            async Task<backend.Models.Product?> Act() {
                return await updateProductUseCase.Execute(updateProductDTO);
            }
            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act());
            Assert.Equal("Produto não existe", exception.Message);
        }

    }
}
