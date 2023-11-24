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

namespace Tests.UnitTests.UseCases.Producer {
    public class GetProducerProductsUseCaseTest:IAsyncLifetime {

        private DatabaseContext _dbContext;

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

            var producer = new CreateProducerDTO {
                Name = "Producer Test",
                Email = "test@test.com",
                AttendedCities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro"
            };

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var productsDTO = new CreateProductDTO[]{
                new CreateProductDTO(
                    "Product",
                    "Description",
                    picture,
                    Category.VEGETABLE,
                    10.10,
                    Unit.UNIT,
                    10,
                    true,
                    "22/11/2023",
                    producerId
                ),
                new CreateProductDTO(
                    "Product2",
                    "Description2",
                    picture,
                    Category.SWEET,
                    23.10,
                    Unit.LITER,
                    2,
                    true,
                    "22/11/2023",
                    producerId
                ),
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

            var producer = new CreateProducerDTO {
                Name = "Producer Test",
                Email = "test@test.com",
                AttendedCities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro"
            };

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

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Guid producerId = Guid.NewGuid();

            var productsDTO = new CreateProductDTO[]{
                new CreateProductDTO(
                    "Product",
                    "Description",
                    picture,
                    Category.VEGETABLE,
                    10.10,
                    Unit.UNIT,
                    10,
                    true,
                    "22/11/2023",
                    producerId
                ),
                new CreateProductDTO(
                    "Product2",
                    "Description2",
                    picture,
                    Category.SWEET,
                    23.10,
                    Unit.LITER,
                    2,
                    true,
                    "22/11/2023",
                    producerId
                ),
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
