using Amazon.S3.Model;
using backend.Picture.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using backend.ProducerPicture.DTOs;

namespace backend.ProducerPicture.Services {
    public interface IProducerPictureDTO {
        Task<List<PutObjectResponse>> UploadImageAsync(List<CreateProducerPictureDTO> pictures, Models.Producer product);

        Task<List<string>> GetImagesAsync(Models.Producer producer);

        Task<DeleteObjectResponse> DeleteImageAsync(Guid imageKey);
    }
}
