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
        public void GetProducerProducts_GivenProducerId_ReturnsProducerProducts()
        {
            //Arrange
            _productRepository.Setup(x => x.GetProducerProducts(It.IsAny<Guid>())).Returns(new List<backend.Models.Product>());
            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(_productRepository.Object);

            //Act
            var foundProducts = getProducerProductsUseCase.Execute(Guid.NewGuid());

            //Assert
            Assert.NotNull(foundProducts);
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task GetProducerProducts_GivenNotExistentProducerId_ThrowsError()
        {

            //Arrange
            _producerRepository.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<Producer?>(null));
            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(_productRepository.Object);

            //Act
            async Task Act()
            {
                var foundProducts = await getProducerProductsUseCase.Execute(Guid.NewGuid());
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act());
            Assert.Equal("Produtor não existe", exception.Message);
        }

    }
}
