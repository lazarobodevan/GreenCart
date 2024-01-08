using backend.Contexts;
using backend.Picture.Repository;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Shared.Factories.Picture;
using backend.Models;

namespace UnitTests.UnitTests.Repositories {
    public class PictureRepositoryTest : IAsyncLifetime{

        private DbContextOptions<DatabaseContext> _dbContextOptions;

        DatabaseContext _databaseContext;

        public PictureRepositoryTest() {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "PictureRepositoryTest").Options;
            _databaseContext = new DatabaseContext(_dbContextOptions);
        }

        public Task DisposeAsync() {
            //Reset database
            return _databaseContext.Database.EnsureDeletedAsync();

        }

        public Task InitializeAsync() {
            //throw new NotImplementedException();
            return _databaseContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task Create_GivenPictureEntity_ReturnsCreatedImage() {
            //Arrange
            var pictureRepository = new PictureRepository(_databaseContext);
            var pictures = new List<backend.Models.Picture>() {
                new PictureFactory().Build(),
                new PictureFactory().Build()
            };
            var producerId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            //Act
            var createdPictures = await pictureRepository.Create(pictures);

            //Assert
            Assert.Equal(2, createdPictures.Count());
        }

        [Fact]
        public async Task Create_GivenPictureEntity_ThrowsDatabaseException() {
            //Arrange
            var mockContext = new Mock<DatabaseContext>(_dbContextOptions);
            var pictureRepository = new PictureRepository(mockContext.Object);
            var pictures = new List<Picture>() { new Picture(), new Picture() };

            mockContext.Setup(x => x.Pictures.AddAsync(It.IsAny<Picture>(), It.IsAny<CancellationToken>())).ThrowsAsync(It.IsAny<Exception>());

            //Act
            async Task _Act(){
                var createdPictures = await pictureRepository!.Create(pictures);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _Act());
            Assert.Equal("Falha ao salvar imagem no banco de dados",exception.Message);
        }

        [Fact]
        public async Task Update_GivenThreePicturesChangeOrderFirstWithSecond_ReturnsReorderedPictures() {
            //Arrange
            var picturesRepository = new PictureRepository(_databaseContext);
            var picturesId = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            var productId = Guid.NewGuid();
            var producerId = Guid.NewGuid();
            var pictures = new List<Picture>() {
                new PictureFactory()
                    .WithKey(picturesId.ElementAt(0))
                    .WithProductId(productId)
                    .Build(),
                new PictureFactory()
                    .WithKey(picturesId.ElementAt(1))
                    .WithProductId(productId)
                    .WithPosition(1)
                    .Build(),
                new PictureFactory()
                    .WithKey(picturesId.ElementAt(2))
                    .WithProductId(productId)
                    .WithPosition(2)
                    .Build()
            };
            
            var pictureToSwitch = new List<Picture>(){  
                new PictureFactory()
                    .WithKey(picturesId.ElementAt(0))
                    .WithProductId(productId)
                    .WithPosition(1)
                    .Build()
            };

            // Picture A (1) goes to Position 0 (B)
            // Then Picture B goes to 0 and A goes to 1
            var expected = new List<Picture>() {
                new PictureFactory()
                    .WithKey(picturesId.ElementAt(1))
                    .WithPosition(0)
                    .WithProductId(productId)
                    .Build(),
                new PictureFactory()
                    .WithKey(picturesId.ElementAt(0))
                    .WithProductId(productId)
                    .WithPosition(1)
                    .Build(),
                new PictureFactory()
                    .WithKey(picturesId.ElementAt(2))
                    .WithProductId(productId)
                    .WithPosition(2)
                    .Build()
            };

            //Act
            var createdPictures = await picturesRepository.Create(pictures);
            var switchedPictures = picturesRepository.Update(pictureToSwitch);

            //Assert
            for(int i = 0; i < pictures.Count; i++) {
                Assert.Equal(expected.ElementAt(i).Key, switchedPictures.ElementAt(i).Key);
                Assert.Equal(expected.ElementAt(i).Position, switchedPictures.ElementAt(i).Position);
            }
        }

        [Fact]
        public async Task Delete_GivenPictureKey_ReturnsDeletedPicture() {
            //Arrange
            var productId = Guid.NewGuid();
            var pictures = new List<Picture>() { 
                new PictureFactory()
                    .WithProductId(productId)
                    .Build() 
            };
            var pictureRepository = new PictureRepository(_databaseContext);

            //Act
            var createdPictures = await pictureRepository.Create(pictures);
            var deletedPicture = pictureRepository.Delete(createdPictures.ElementAt(0).Key);
            var picturesFromProduct = pictureRepository.FindPicturesFromProduct(productId);

            //Assert
            Assert.Equal(createdPictures.ElementAt(0).Key, deletedPicture.Key);
            Assert.Empty(picturesFromProduct);
        }

        [Fact]
        public async Task Find_GivenProductId_ReturnsPicturesFromProduct() {
            //Arrange
            var pictureRepository = new PictureRepository(_databaseContext);
            var productId = Guid.NewGuid();
            var pictures = new List<backend.Models.Picture>() {
                new PictureFactory()
                    .WithProductId(productId)
                    .Build(),
                new PictureFactory()
                    .WithProductId(productId)
                    .Build()
            };

            //Act
            var createdPictures = await pictureRepository.Create(pictures);
            var foundPictures = pictureRepository.FindPicturesFromProduct(productId);

            //Assert
            Assert.Equal(2, foundPictures.Count());
        }

    }
}
