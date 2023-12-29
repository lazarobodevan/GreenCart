using Amazon.S3.Model;
using backend.Picture.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Producer.Services {
    public interface IPictureService {

        Task<List<PutObjectResponse>> UploadImageAsync(List<CreatePictureDTO> pictures, Models.Product product);

        Task<List<string>> GetImagesAsync(Models.Product product);
    }
}
