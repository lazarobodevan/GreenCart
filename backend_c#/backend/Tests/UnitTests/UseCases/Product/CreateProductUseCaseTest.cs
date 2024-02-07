using Amazon.S3.Model;
using backend.Models;
using backend.Picture.DTOs;
using backend.Picture.Repository;
using backend.Producer.Services;
using backend.Product.DTOs;
using backend.Product.Repository;
using backend.Product.UseCases;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories.Product;

namespace Tests.UnitTests.UseCases
{
    public class CreateProductUseCaseTest {

        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IProductPictureService> _pictureServiceMock;

        public CreateProductUseCaseTest() {
            _productRepositoryMock = new Mock<IProductRepository>();
            _pictureServiceMock = new Mock<IProductPictureService>();
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenCreateProductDTO_ReturnsCreatedProduct() {
            //Arrange
            _productRepositoryMock.Setup(
                x => x.Save(It.IsAny<backend.Models.Product>(), new List<CreateProductPictureDTO>())
            ).ReturnsAsync(new ProductFactory().Build());

            _pictureServiceMock.Setup(x => 
                x.UploadImageAsync(new List<CreateProductPictureDTO>(), It.IsAny<backend.Models.Product>())
            ).ReturnsAsync(new List<PutObjectResponse>());

            CreateProductUseCase createProductUseCase = new CreateProductUseCase(_productRepositoryMock.Object, _pictureServiceMock.Object);
            var productDTO = new ProductDTOFactory().Build();
            
            //Act
            var product =  await createProductUseCase.Execute(productDTO);

            //Assert
            Assert.NotNull(product);
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenCreateProductDTOWithInvalidHarvestDate_ThrowsException() {
            
            //Arrange

            CreateProductUseCase createProductUseCase = new CreateProductUseCase(_productRepositoryMock.Object, _pictureServiceMock.Object);
            CreateProductDTO productDTO = new ProductDTOFactory()
                    .WithHarvestDate("31/31/1231")
                    .Build();

            //Act
            async Task Act() {
                var createdProduct = await createProductUseCase.Execute(productDTO);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act());
            Assert.Equal("Formato inválido de data", exception.Message);
        }
    }
}
