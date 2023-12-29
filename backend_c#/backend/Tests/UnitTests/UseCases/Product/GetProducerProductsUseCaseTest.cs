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

namespace UnitTests.UnitTests.UseCases.Product
{
    public class GetProducerProductsUseCaseTest
    {

        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IProducerRepository> _producerRepository;

        public GetProducerProductsUseCaseTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _producerRepository = new Mock<IProducerRepository>();
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task GetProducerProducts_GivenProducerId_ReturnsPaginatedProducts()
        {
            //Arrange

            var storedProducts = new ListDatabaseProductsPagination() {
                Pages = 1,
                CurrentPage = 0,
                Products = new List<backend.Models.Product>() {
                    new ProductFactory().WithName("1").Build(),
                    new ProductFactory().WithName("2").Build(),
                }
            };
            _productRepository.Setup(x => x.GetProducerProducts(It.IsAny<Guid>(), 0, 2)).Returns(storedProducts);
            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(_productRepository.Object);

            //Act
            var foundProductsPage0 = await getProducerProductsUseCase.Execute(Guid.NewGuid(), 0, 2);

            //Assert
            Assert.NotNull(foundProductsPage0);
            Assert.Equal(2, foundProductsPage0.Products.Count());
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task GetProducerProducts_GivenNotExistentProducerId_ThrowsProducerDoesNotExistException()
        {

            //Arrange
            _productRepository.Setup(x => x.GetProducerProducts(
                It.IsAny<Guid>(), 
                It.IsAny<int>(), 
                It.IsAny<int>())
            ).Throws(new ProducerDoesNotExistException());

            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(_productRepository.Object);

            //Act
            async Task Act()
            {
                var foundProducts = await getProducerProductsUseCase.Execute(Guid.NewGuid(), 0, 5);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<ProducerDoesNotExistException>(async () => await Act());
            Assert.Equal("Produtor não existe", exception.Message);
        }

    }
}
