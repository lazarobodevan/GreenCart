using Amazon.S3.Model;
using backend.Picture.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using backend.ProducerPicture.DTOs;

namespace backend.ProducerPicture.Services {
    public interface IProducerPictureService {
        Task<PutObjectResponse> UploadProfilePictureAsync(Models.Producer producer, CreateProducerPictureDTO pictureDTO);

        Task<string> GetProfilePictureAsync(Models.Producer producer);

        Task<DeleteObjectResponse> DeleteImageAsync(Guid imageKey);
    }
}
