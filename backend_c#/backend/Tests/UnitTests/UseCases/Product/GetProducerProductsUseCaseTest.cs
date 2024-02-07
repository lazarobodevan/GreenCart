using backend.Contexts;
using backend.Models;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Product.Repository;
using Tests.Factories;
using backend.Product.UseCases;
using backend.Product.DTOs;
using Tests.Factories.Product;
using backend.Product.Exceptions;
using backend.Producer.Services;
using backend.Shared.Classes;

namespace UnitTests.UnitTests.UseCases.Product
{
    public class GetProducerProductsUseCaseTest
    {

        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IProducerRepository> _producerRepository;
        private readonly Mock<IProductPictureService> _pictureServiceMock;

        public GetProducerProductsUseCaseTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _producerRepository = new Mock<IProducerRepository>();
            _pictureServiceMock = new Mock<IProductPictureService>();
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task GetProducerProducts_GivenProducerId_ReturnsPaginatedProducts()
        {
            //Arrange

            var storedProducts = new Pagination<backend.Models.Product>() {
                Pages = 1,
                CurrentPage = 0,
                Offset = 0,
                Data = new List<backend.Models.Product>() {
                    new ProductFactory().WithName("1").Build(),
                    new ProductFactory().WithName("2").Build(),
                }
            };
            _productRepository.Setup(x => x.GetProducerProducts(It.IsAny<Guid>(), 0, 10, null)).Returns(storedProducts);
            _pictureServiceMock.Setup(x => x.GetImagesAsync(It.IsAny<backend.Models.Product>())).ReturnsAsync(new List<string>() { "link1", "link2"});

            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(_productRepository.Object, _pictureServiceMock.Object);

            //Act
            var foundProductsPage0 = await getProducerProductsUseCase.Execute(Guid.NewGuid(), 0, null);

            //Assert
            Assert.NotNull(foundProductsPage0);
            Assert.Equal(2, foundProductsPage0.Data.Count());
            Assert.Single(foundProductsPage0.Data.ElementAt(0).Pictures);
            Assert.Single(foundProductsPage0.Data.ElementAt(1).Pictures);
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task GetProducerProducts_GivenNotExistentProducerId_ThrowsProducerDoesNotExistException()
        {

            //Arrange
            _productRepository.Setup(x => x.GetProducerProducts(
                It.IsAny<Guid>(), 
                It.IsAny<int>(), 
                It.IsAny<int>(),
                null)
            ).Throws(new ProducerDoesNotExistException());

            _pictureServiceMock.Setup(x => x.GetImagesAsync(It.IsAny<backend.Models.Product>())).ReturnsAsync(new List<string>() { "link1", "link2"});

            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(_productRepository.Object, _pictureServiceMock.Object);

            //Act
            async Task Act()
            {
                var foundProducts = await getProducerProductsUseCase.Execute(Guid.NewGuid(), 0, null);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<ProducerDoesNotExistException>(async () => await Act());
            Assert.Equal("Produtor não existe", exception.Message);
        }

    }
}
