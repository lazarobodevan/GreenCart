using Amazon.S3;
using Amazon.S3.Model;
using backend.Models;
using backend.ProducerPicture.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.ProducerPicture.Services {
    public class ProducerPictureService : IProducerPictureService {

        private readonly IAmazonS3 _amazonS3;
        private readonly string _BucketName;

        public ProducerPictureService(IAmazonS3 amazonS3, string bucketName) {
            _amazonS3 = amazonS3;
            _BucketName = bucketName;
        }

        public async Task<DeleteObjectResponse> DeleteImageAsync(Guid imageKey) {
            try {
                var deleteObjectRequest = new DeleteObjectRequest {
                    BucketName = _BucketName,
                    Key = imageKey.ToString()
                };

                return await _amazonS3.DeleteObjectAsync(deleteObjectRequest);
            } catch (AmazonS3Exception ex) {
                throw new AmazonS3Exception($"Falha ao deletar objeto: {ex.Message}");
            }
        }

        public async Task<string?> GetProfilePictureAsync(Models.Producer producer) {
            try {
                if (!producer.HasProfilePicture) return null;
                var getObjectRequest = new GetPreSignedUrlRequest {
                    BucketName = _BucketName,
                    Key = $"{producer.Id}/{producer.Id}",
                    Expires = DateTime.UtcNow.AddHours(5),
                    Protocol = Protocol.HTTPS,
                };
                var foundPicture = await _amazonS3.GetPreSignedURLAsync(getObjectRequest);
                return foundPicture;
            } catch (AmazonS3Exception ex) {
                throw new Exception($"Erro ao obter foto de perfil: {ex.Message}");
            }
        }

        public async Task<PutObjectResponse> UploadProfilePictureAsync(Models.Producer producer, CreateProducerPictureDTO pictureDTO) {
            List<PutObjectResponse> picturesObjectResponse = new List<PutObjectResponse>();
            var putObjectRequest = new PutObjectRequest {
                BucketName = _BucketName,
                Key = $"{producer.Id}/{producer.Id}",
                ContentType = "image/png",
                InputStream = pictureDTO.Stream
            };
            var response = await _amazonS3.PutObjectAsync(putObjectRequest);

            return response;
        }
    }
}
