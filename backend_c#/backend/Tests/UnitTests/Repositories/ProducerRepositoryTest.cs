using backend.Contexts;
using backend.Models;
using backend.Picture.DTOs;
using backend.Producer.Repository;
using backend.Product.Enums;
using backend.Product.Repository;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Shared.Factories.Picture;

//***** DO NOT REMOVE*****
[assembly: CollectionBehavior(DisableTestParallelization = false)]
//************************

namespace Tests.UnitTests.Repositories
{

    public class ProducerRepositoryTest {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _dbContext;
        private readonly IProducerRepository _producerRepository;
        private readonly IProductRepository _productRepository;


        public ProducerRepositoryTest() {
            _options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("test_database").Options;
            _dbContext = new DatabaseContext(_options);
            _dbContext.Database.EnsureDeleted();
            _producerRepository = new ProducerRepository(_dbContext);
            this._productRepository = new ProductRepository(_dbContext);
        }


        [Fact]
        [Trait("OP","Create")]
        public async Task Save_GivenProducerEntity_ReturnsCreatedProducerEntity() {
            
            //Arrange
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro",
                Location = new Point(new Coordinate(-44.436909544476336, -19.8959282622104))
            };

            

            //Act
            var createdProducer = await _producerRepository.Save(producer);

            //Assert
            Assert.NotNull(createdProducer);
            Assert.IsType<Producer>(createdProducer);
            Assert.False(Guid.Empty.Equals(createdProducer.Id));
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenInvalidProducerEntity_ThrowsException() {
            //Arrange
            // Missing fields
            var producer = new Producer {
               
            };

            //Act
            async Task Act() {
                var createdProducer = await _producerRepository.Save(producer);
            }

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async() => await Act());
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenProducerId_ReturnsProducerEntity() {
            //Arrange
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro",
                Location = new Point(new Coordinate(-44.436909544476336, -19.8959282622104))
            };

            //Act
            _dbContext.ChangeTracker.Clear();
            var createdProducer = await _producerRepository.Save(producer);
            _dbContext.ChangeTracker.Clear();
            var possibleProducer = await _producerRepository.FindById(createdProducer.Id);

            //Assert
            Assert.NotNull(possibleProducer);
            Assert.False(Guid.Empty.Equals(createdProducer.Id));
        }

        [Fact]
        [Trait("OP", "FindById")]
        public async Task FindById_GivenInvalidProducerId_ReturnsNull() {
            //Arrange
            var producerId = Guid.NewGuid();

            //Act
            var possibleProducer = await _producerRepository.FindById(producerId);

            //Assert
            Assert.Null(possibleProducer);
        }

        [Fact]
        [Trait("OP", "FindByEmail")]
        public async Task FindByEmail_GivenEmail_ReturnsProducerEntity() {
            //Arrange
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro",
                Location = new Point(new Coordinate(-44.436909544476336, -19.8959282622104))
            };

            //Act
            var createdProducer = await _producerRepository.Save(producer);
            var possibleProducer = await _producerRepository.FindByEmail(createdProducer.Email);

            //Assert
            Assert.NotNull(possibleProducer);
            Assert.Equal(possibleProducer.Email, createdProducer.Email);
        }

        [Fact]
        [Trait("OP", "FindByEmail")]
        public async Task FindByEmail_GivenNotExistentEmail_ReturnsNull() {
            //Arrange
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro",
                Location = new Point(new Coordinate(-44.436909544476336, -19.8959282622104))
            };

            var fakeEmail = "test@fake.com";

            //Act
            var createdProducer = await _producerRepository.Save(producer);
            var possibleProducer = await _producerRepository.FindByEmail(fakeEmail);

            //Assert
            Assert.Null(possibleProducer);
        }

        /*
         * Must be in an integration test
         * 
        [Fact]
        [Trait("OP", "FindNearProducers")]
        public async Task FindNearProducers_GivenCityOfFlorestal_ReturnsOneProducersNearby() {
            //Arrange

            Point florestal = new(new Coordinate() {
                X = -44.43324160008131,
                Y = -19.88818771861369,
            });

            backend.Shared.Classes.Location myLocation = new() {
                Latitude = -19.8959282622104,
                Longitude = -44.436909544476336,
                RadiusInKm = 1
            };

            //Para de Minas is about 20Km far from Florestal
            Point parademinas = new Point(new Coordinate() {
                X = -44.59249910032828,
                Y = -19.863470386911303,
            });

            var producer1Florestal = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro",
                Location = florestal
            };
            var producer2ParaDeMinas = new Producer {
                Name = "Producer Test2",
                Email = "test2@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9990",
                WhereToFind = "Local de encontro2",
                Location = parademinas
            };

            var createdProducer1 = await _producerRepository.Save(producer1Florestal);
            var createdProducer2 = await _producerRepository.Save(producer2ParaDeMinas);

            //Act
            var producersFromCityFlorestal = _producerRepository.FindNearProducers(myLocation,0,10, null);

            Assert.Single(producersFromCityFlorestal.Data);
        }
        */

        [Fact]
        [Trait("OP", "Update")]
        public async Task Update_GivenProducer_ReturnsUpdatedProducer() {
            //Arrange
            var producer = new Producer {
                Id = Guid.NewGuid(),
                Name = "Producer Test",
                Email = "test@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro",
                Location = new Point(new Coordinate(-44.436909544476336, -19.8959282622104))
            };


            //Act
            var createdProducer = await _producerRepository.Save(new backend.Models.Producer (producer));
            
            _dbContext.ChangeTracker.Clear();

            producer.Id = createdProducer.Id;
            producer.Name = "Test";
            producer.Telephone = "(31) 99999-9991";
            producer.WhereToFind = "Local";
            producer.Email = "test2@test.com";
            producer.Password = "321";

            var updatedProducer = await _producerRepository.Update(producer);

            //Assert
            Assert.NotNull(createdProducer);
            Assert.IsType<Producer>(createdProducer);
            
            Assert.Equal(updatedProducer.Id, createdProducer.Id);
            Assert.NotEqual(DateTime.MinValue, updatedProducer.UpdatedAt);
            Assert.NotEqual(updatedProducer.Name, createdProducer.Name);
            Assert.NotEqual(updatedProducer.Telephone, createdProducer.Telephone);
            Assert.NotEqual(updatedProducer.WhereToFind, createdProducer.WhereToFind);
            Assert.NotEqual(updatedProducer.Email, createdProducer.Email);
            Assert.NotEqual(updatedProducer.Password, createdProducer.Password);
        }

        [Fact]
        [Trait("OP", "Delete")]
        public async Task Delete_GivenProducer_ReturnsDeletedProducer() {
            //Arrange
            var producer = new Producer {
                Name = "Producer Test",
                Email = "test@test.com",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro",
                Location = new Point(new Coordinate(-44.436909544476336, -19.8959282622104))
            };

            //Act
            var createdProducer = await _producerRepository.Save(producer);
            Assert.NotNull(createdProducer);

            var deletedProducer = await _producerRepository.Delete(createdProducer);

            //Assert
            Assert.NotNull(deletedProducer);
            Assert.NotEqual(DateTime.MinValue, createdProducer.DeletedAt) ;
        }

        [Fact]
        [Trait("OP","Delete")]
        public async Task Delete_GivenNotExistentProducer_ThrowsException() {
            //Arrange
            var producerId = Guid.NewGuid();
            var producer = new backend.Models.Producer {
                Id = producerId
            };

            //Act
            async Task Act(backend.Models.Producer producer) {
                await _producerRepository.Delete(producer);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(async () => await Act(producer));
        }
    }
}
