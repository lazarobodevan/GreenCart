using Amazon.S3.Model;
using backend.Picture.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Producer.Services {
    public interface IProductPictureService {

        Task<List<PutObjectResponse>> UploadImageAsync(List<CreateProductPictureDTO> pictures, Models.Product product);

        Task<List<string>> GetImagesAsync(Models.Product product);

        Task<DeleteObjectResponse> DeleteImageAsync(Guid imageKey);
    }
}
