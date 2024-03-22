using backend.Picture.Repository;
using backend.Picture.UseCases;
using backend.Producer.Services;
using backend.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Picture.DTOs;
using Tests.Shared.Factories.Picture;
using backend.Picture.Exceptions;
using Amazon.S3;
using backend.Product.Repository;

namespace UnitTests.UnitTests.UseCases.Picture {
    public class CreatePicturesUseCaseTest {
        
        private Mock<IPictureRepository> _pictureRepositoryMock;
        private Mock<IProductPictureService> _pictureServiceMock;
        private Mock<IProductRepository> _productRepositoryMock;

        public CreatePicturesUseCaseTest() {
            _pictureRepositoryMock = new Mock<IPictureRepository>();
            _pictureServiceMock = new Mock<IProductPictureService>();
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task Create_GivenListOfCreatePictureDTO_ReturnsListOfPictureEntities() {
            //Arrange
            CreatePicturesUseCase createPicturesUseCase = new CreatePicturesUseCase(_pictureRepositoryMock.Object, _pictureServiceMock.Object, _productRepositoryMock.Object);
            var productId = Guid.NewGuid();
            var storedPictures = new List<backend.Models.ProductPicture>() {
                new PictureFactory()
                    .WithProductId(productId)
                    .Build(),
                new PictureFactory()
                    .WithProductId(productId)
                    .WithPosition(1)
                    .Build(),
            };

            var newPicture = new List<backend.Models.ProductPicture>() {
                new PictureFactory()
                    .WithProductId(productId)
                    .WithPosition(2)
                    .Build(),
            };

            _pictureRepositoryMock.Setup(x => 
                x.FindPicturesFromProduct(It.IsAny<Guid>()))
                .Returns(storedPictures);

            _pictureRepositoryMock.Setup(x => 
                x.Create(It.IsAny<List<backend.Models.ProductPicture>>()))
                .ReturnsAsync(newPicture);

            _pictureServiceMock.Setup(x => 
                x.UploadImageAsync(It.IsAny<List<CreateProductPictureDTO>>(), It.IsAny<backend.Models.Product>()))
                .ReturnsAsync(new List<Amazon.S3.Model.PutObjectResponse>()
            );

            //Act
            var createdPictures = await createPicturesUseCase.Execute(new List<CreateProductPictureDTO>(), Guid.NewGuid());

            //Assert
            Assert.Single(createdPictures);
        }

        [Fact]
        public async Task Create_GivenListOfCreatePictureDTOWithConflictingPositions_ThrowsConflictingPositionsException() {
            //Arrange
            CreatePicturesUseCase createPicturesUseCase = new CreatePicturesUseCase(_pictureRepositoryMock.Object, _pictureServiceMock.Object, _productRepositoryMock.Object);
            var productId = Guid.NewGuid();
            var storedPictures = new List<backend.Models.ProductPicture>() {
                new PictureFactory()
                    .WithProductId(productId)
                    .Build(),
                new PictureFactory()
                    .WithProductId(productId)
                    .WithPosition(1)
                    .Build(),
            };

            var newPictures = new List<CreateProductPictureDTO>() {
                new CreatePictureDTOFactory().Build(),
                new CreatePictureDTOFactory().Build()
            };

            _pictureRepositoryMock.Setup(x =>
                x.FindPicturesFromProduct(It.IsAny<Guid>()))
                .Returns(storedPictures);

            //Act
            async Task Act() {
                var createdPictures = await createPicturesUseCase.Execute(newPictures, Guid.NewGuid());
            }

            //Assert
            var exception = await Assert.ThrowsAsync<ConflictingPositionsException>(async () => await Act());
            Assert.Equal("Já existe uma imagem na posição 0", exception.Message);
        }

        [Fact]
        public async Task Create_GivenListOfCreatePictureDTO_ThrowsAmazonS3Exception() {
            //Arrange
            CreatePicturesUseCase createPicturesUseCase = new CreatePicturesUseCase(_pictureRepositoryMock.Object, _pictureServiceMock.Object, _productRepositoryMock.Object);
            var productId = Guid.NewGuid();
            var storedPictures = new List<backend.Models.ProductPicture>() {
                new PictureFactory()
                    .WithProductId(productId)
                    .Build(),
                new PictureFactory()
                    .WithProductId(productId)
                    .WithPosition(1)
                    .Build(),
            };

            var newPictures = new List<CreateProductPictureDTO>() {
                new CreatePictureDTOFactory()
                    .WithPosition(3)
                    .Build(),
                new CreatePictureDTOFactory()
                    .WithPosition(4)
                    .Build()
            };

            _pictureRepositoryMock.Setup(x =>
                x.FindPicturesFromProduct(It.IsAny<Guid>()))
                .Returns(storedPictures);

            _pictureServiceMock.Setup(x => 
                x.UploadImageAsync(It.IsAny<List<CreateProductPictureDTO>>(), It.IsAny<backend.Models.Product>()))
                .ThrowsAsync(new AmazonS3Exception("Erro"));

            //Act
            async Task Act() {
                var createdPictures = await createPicturesUseCase.Execute(newPictures, Guid.NewGuid());
            }

            //Assert
            var exception = await Assert.ThrowsAsync<AmazonS3Exception>(async () => await Act());
            Assert.Equal("Falha ao fazer upload da imagem: Erro", exception.Message);
        }

    }
}
