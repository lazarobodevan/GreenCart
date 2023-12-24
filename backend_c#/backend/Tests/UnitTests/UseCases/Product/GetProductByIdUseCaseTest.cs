using backend.Contexts;
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

        public GetProductByIdUseCaseTest() {
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProductId_ReturnsFoundProduct() {

            //Arrange
            _productRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(new ProductFactory().Build());

            GetProductByIdUseCase getProductByIdUseCase = new GetProductByIdUseCase(_productRepositoryMock.Object);
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
            _productRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>())).Returns(Task.FromResult<backend.Models.Product?>(null));

            GetProductByIdUseCase getProductByIdUseCase = new GetProductByIdUseCase(_productRepositoryMock.Object);
            ProductDTOFactory productFactory = new ProductDTOFactory();

            //Act
            var possibleProduct = await getProductByIdUseCase.Execute(Guid.NewGuid());

            //Assert
            Assert.Null(possibleProduct);
        }
    }
}
