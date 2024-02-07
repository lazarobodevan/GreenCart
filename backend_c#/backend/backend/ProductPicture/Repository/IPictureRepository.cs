using backend.Picture.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Picture.Repository {
    public interface IPictureRepository {
        Task<List<Models.ProductPicture>> Create(List<Models.ProductPicture> pictures);
        List<Models.ProductPicture> Update(List<Models.ProductPicture> picture);
        Models.ProductPicture Delete(Guid pictureId);
        List<Models.ProductPicture> FindPicturesFromProduct(Guid productId);
    }
}
