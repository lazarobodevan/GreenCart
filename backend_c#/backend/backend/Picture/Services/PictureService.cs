using Amazon.S3;
using Amazon.S3.Model;
using backend.Picture.DTOs;
using backend.Product.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Producer.Services {
    public class PictureService : IPictureService {

        private readonly IAmazonS3 _amazonS3;
        private readonly string _BucketName;

        public PictureService(IAmazonS3 amazonS3, string bucketName) {
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
            } catch(AmazonS3Exception ex) {
                throw new AmazonS3Exception($"Falha ao deletar objeto: {ex.Message}");
            }
            
            
        }

        public async Task<List<string>> GetImagesAsync(Models.Product product) {
            try {
                List<string> awsPictures = new List<string>();
                foreach (var picture in product.Pictures) {
                    var getObjectRequest = new GetPreSignedUrlRequest {
                        BucketName = _BucketName,
                        Key = $"{product.ProducerId}/products/{product.Id}/{picture.Key}",
                        Expires = DateTime.UtcNow.AddHours(5),
                        Protocol = Protocol.HTTPS,
                        
                    };
                    
                    var pictureGotten = await _amazonS3.GetPreSignedURLAsync(getObjectRequest);
                    awsPictures.Add(pictureGotten);
                }
                return awsPictures;
            }catch(AmazonS3Exception ex) {
                return new List<string>();
            }
        }

        public async Task<List<PutObjectResponse>> UploadImageAsync(List<CreatePictureDTO> pictures, Models.Product product) {

            List<PutObjectResponse> picturesObjectResponse = new List<PutObjectResponse>();
            foreach (var picture in pictures) {
                var putObjectRequest = new PutObjectRequest {
                    BucketName = _BucketName,
                    Key = $"{product.ProducerId}/products/{product.Id}/{picture.Key}",
                    ContentType = "image/png",
                    InputStream = picture.Stream
                };
                var response = await _amazonS3.PutObjectAsync(putObjectRequest);
                picturesObjectResponse.Add(response);
            }
            return picturesObjectResponse;
        }
    }
}
