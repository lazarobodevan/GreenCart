using Amazon.S3;
using backend.Models;
using backend.Picture.DTOs;
using backend.Picture.Exceptions;
using backend.Picture.Repository;
using backend.Producer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Picture.UseCases {
    public class CreatePicturesUseCase {
        private readonly IPictureRepository _pictureRepository;
        private readonly IPictureService _pictureService;
        private const int MAX_NUMBER_OF_PICTURES_PER_PRODUCT = 5;

        public CreatePicturesUseCase(IPictureRepository pictureRepository, IPictureService pictureService) {
            _pictureRepository = pictureRepository;
            _pictureService = pictureService;
        }

        public async Task<List<Models.Picture>> Execute(List<CreatePictureDTO> picturesDTO, Models.Product product) {
            var storedPictures = _pictureRepository.FindPicturesFromProduct(product.Id);

            if(storedPictures.Count >= MAX_NUMBER_OF_PICTURES_PER_PRODUCT) {
                throw new ExceededNumberOfPicturesException();
            }
            
            foreach(var picture in picturesDTO) {
                _CheckConflictingPositions(picture, storedPictures);
            }
            
            var picturesEntities = _GeneratePictureEntities(picturesDTO, storedPictures);

            try {
                var uploadedPictures = await _pictureService.UploadImageAsync(picturesDTO, product);
                var createdPictures = await _pictureRepository.Create(picturesEntities);

                return createdPictures;

            } catch(AmazonS3Exception ex) {
                throw new AmazonS3Exception($"Falha ao fazer upload da imagem: {ex.Message}");
            
            } catch(Exception ex) {
                await _RollbackS3(picturesEntities);
                throw new Exception($"{ex.Message}");
            }

        }

        private List<Models.Picture> _GeneratePictureEntities(List<CreatePictureDTO> pictureDTO, List<Models.Picture> storedPictures) {
            
            List<Models.Picture> picturesEntities = new List<Models.Picture>();
            var productId = storedPictures.ElementAt(0).ProductId;

            foreach (var picture in pictureDTO) {

                picturesEntities.Add(new Models.Picture() {
                    Key = Guid.NewGuid(),
                    Position = (int)picture.Position!,
                    ProductId = productId,
                });
            }

            return picturesEntities;
        }

        private void _CheckConflictingPositions(CreatePictureDTO newPicture, List<Models.Picture> storedPictures) {

            var hasPictureInPosition = storedPictures.Find(p => p.Position == newPicture.Position);
            if (hasPictureInPosition != null) {
                throw new ConflictingPositionsException((int)newPicture.Position!);
            }
        }

        private async Task _RollbackS3(List<Models.Picture> picturesEntities) {
            foreach (var picture in picturesEntities) {
                await _pictureService.DeleteImageAsync(picture.Key);
            }
        }
       
    }
}
