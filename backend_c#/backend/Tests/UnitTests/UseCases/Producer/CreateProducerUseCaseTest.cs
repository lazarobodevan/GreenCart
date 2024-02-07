using backend.Contexts;
using backend.Models;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using backend.ProducerPicture.DTOs;
using backend.ProducerPicture.Services;
using backend.Utils.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Factories.Producer;

namespace Tests.UnitTests.UseCases.Producer
{

    public class CreateProducerUseCaseTest {

        private ProducerDTOFactory producerFactory = new ProducerDTOFactory();
        private Mock<IProducerRepository> producerRepository;
        private Mock<IProducerPictureService> producerPictureService;

        public CreateProducerUseCaseTest() {
            producerRepository = new Mock<IProducerRepository>();
            producerPictureService = new Mock<IProducerPictureService>();
        }


        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenProducer_ReturnsCreatedProducer() {

            //Arrange
            producerRepository.Setup(x => x.Save(It.IsAny<backend.Models.Producer>())).ReturnsAsync(new backend.Models.Producer());
            producerRepository.Setup(x => x.FindByEmail(It.IsAny<string>())).Returns(Task.FromResult<backend.Models.Producer?>(null));
            producerPictureService.Setup(x => x.UploadProfilePictureAsync(It.IsAny<backend.Models.Producer>(), It.IsAny<CreateProducerPictureDTO>())).ReturnsAsync(new Amazon.S3.Model.PutObjectResponse());

            CreateProducerUseCase usecase = new CreateProducerUseCase(producerRepository.Object, producerPictureService.Object);

            var producer = producerFactory.Build();

            //Act
            var createdProducer = await usecase.Execute(producer);

            //Assert
            Assert.NotNull(createdProducer);
        }

        [Fact]
        [Trait("OP", "Create")]
        public async Task Save_GivenAlreadyExistentProducer_ThrowsError() {
            //Arrange
            producerRepository.Setup(x => x.Save(It.IsAny<backend.Models.Producer>())).ReturnsAsync(new backend.Models.Producer());
            producerRepository.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(new backend.Models.Producer());
            producerPictureService.Setup(x => x.UploadProfilePictureAsync(It.IsAny<backend.Models.Producer>(), It.IsAny<CreateProducerPictureDTO>())).ReturnsAsync(new Amazon.S3.Model.PutObjectResponse());

            CreateProducerUseCase usecase = new CreateProducerUseCase(producerRepository.Object, producerPictureService.Object);

            var producer = producerFactory.Build();

            //Act
            async Task Act(CreateProducerDTO producer) {
                var createdProducer = await usecase.Execute(producer);
            }

            //Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await Act(producer));
            Assert.Equal("Usuário já cadastrado", exception.Message);
        }
    }
}
