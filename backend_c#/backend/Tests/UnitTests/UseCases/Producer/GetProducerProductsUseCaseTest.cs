using backend.Contexts;
using backend.DTOs.Producer;
using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Repositories;
using backend.UseCases.Producer;
using backend.UseCases.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories;

namespace Tests.UnitTests.UseCases.Producer {
    public class GetProducerProductsUseCaseTest:IAsyncLifetime {

        private DatabaseContext _dbContext;
        private ProducerFactory producerFactory = new ProducerFactory();

        public GetProducerProductsUseCaseTest() {
            _dbContext = DbContextFactory.GetDatabaseContext();
        }

        public async Task InitializeAsync() {
            await this._dbContext.Database.EnsureDeletedAsync();
        }

        public async Task DisposeAsync() {
            await this._dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task ShouldGetProducerProductsUseCaseSuccessfully() {

            //Arrange
            ProducerRepository producerRepository = new ProducerRepository(_dbContext);
            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(producerRepository);
            CreateProducerUseCase createProducerUsecase = new CreateProducerUseCase(producerRepository);

            ProductRepository productRepository = new ProductRepository(_dbContext);
            CreateManyProductsUseCase createManyProductsUsecase = new CreateManyProductsUseCase(productRepository);

            ProductFactory productFactory = new ProductFactory();

            var producer = producerFactory.GetDefaultCreateProducerDto();

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var productsDTO = new CreateProductDTO[]{
                productFactory.GetDefaultCreateProductDto(producerId).Build(),
                productFactory.GetDefaultCreateProductDto(producerId)
                    .WithName("Product 2")
                    .WithDescription("Description 2")
                    .Build(),
            };

            //Act
            var createdProducer = await createProducerUsecase.Execute(producer);

            foreach(var prod in productsDTO) {
                prod.ProducerId = createdProducer.Id;
            }

            var createdProducts = await createManyProductsUsecase.Execute(productsDTO);

            var foundProducerProducts = await getProducerProductsUseCase.Execute(createdProducer.Id);

            //Assert
            Assert.Equal(productsDTO.Length, foundProducerProducts.Count());
        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task ShouldReturnEmptyListOfProducerProducts() {
            
            //Arrange

            ProducerRepository producerRepository = new ProducerRepository(_dbContext);
            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(producerRepository);
            CreateProducerUseCase createProducerUsecase = new CreateProducerUseCase(producerRepository);

            var producer = producerFactory.GetDefaultCreateProducerDto();

            //Act
            var createdProducer = await createProducerUsecase.Execute(producer);
            var foundProducts = await getProducerProductsUseCase.Execute(createdProducer.Id);

            //Assert
            Assert.Empty(foundProducts);

        }

        [Fact]
        [Trait("OP", "GetProducerProducts")]
        public async Task ShouldThrowErrorWhenTryingToGetProductsFromNotExistantProducer() {

            //Arrange

            ProducerRepository producerRepository = new ProducerRepository(_dbContext);
            GetProducerProductsUseCase getProducerProductsUseCase = new GetProducerProductsUseCase(producerRepository);

            ProductFactory productFactory = new ProductFactory();

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var productsDTO = new CreateProductDTO[]{
                productFactory.GetDefaultCreateProductDto(producerId).Build(),
                productFactory.GetDefaultCreateProductDto(producerId)
                    .WithName("Product 2")
                    .WithDescription("Description 2")
                    .Build()
            };

            //Act
            async Task Act() {
                var foundProducts = await getProducerProductsUseCase.Execute(producerId);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act());
            Assert.Equal("Produtor não existe", exception.Message);
        }

    }
}
