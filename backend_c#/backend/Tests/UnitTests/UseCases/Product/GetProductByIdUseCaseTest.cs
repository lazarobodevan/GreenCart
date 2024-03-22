using backend.Contexts;
using backend.Producer.Services;
using backend.ProducerPicture.Services;
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
    public class GetProductByIdUseCaseTest {
        
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IProductPictureService> _pictureServiceMock;
        private readonly Mock<IProducerPictureService> _producerPictureService;

        public GetProductByIdUseCaseTest() {
            _productRepositoryMock = new Mock<IProductRepository>();
            _pictureServiceMock = new Mock<IProductPictureService>();
            _producerPictureService = new Mock<IProducerPictureService>();
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProductId_ReturnsFoundProduct() {

            //Arrange
            _productRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(new ProductFactory().Build());
            _pictureServiceMock.Setup(x => x.GetImagesAsync(It.IsAny<backend.Models.Product>())).ReturnsAsync(new List<string>() { "link1" });
            

            GetProductByIdUseCase getProductByIdUseCase = new GetProductByIdUseCase(_productRepositoryMock.Object, _pictureServiceMock.Object, _producerPictureService.Object);
            ProductDTOFactory productFactory = new ProductDTOFactory();

            //Act
            var possibleProduct = await getProductByIdUseCase.Execute(Guid.NewGuid());

            //Assert
            Assert.NotNull( possibleProduct );
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenNotExistentProductId_ReturnsNull() {
            //Arrange
            _productRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).Returns((Guid id) => null);
            _pictureServiceMock.Setup(x => x.GetImagesAsync(It.IsAny<backend.Models.Product>())).ReturnsAsync(new List<string>() { "link1" });

            GetProductByIdUseCase getProductByIdUseCase = new GetProductByIdUseCase(_productRepositoryMock.Object, _pictureServiceMock.Object, _producerPictureService.Object);
            ProductDTOFactory productFactory = new ProductDTOFactory();

            //Act
            var possibleProduct = await getProductByIdUseCase.Execute(Guid.NewGuid());

            //Assert
            Assert.Null(possibleProduct);
        }
    }
}
