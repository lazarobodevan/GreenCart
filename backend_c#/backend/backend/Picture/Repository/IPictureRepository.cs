using backend.Picture.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Picture.Repository {
    public interface IPictureRepository {
        Task<List<Models.Picture>> Create(List<Models.Picture> pictures);
        List<Models.Picture> Update(List<Models.Picture> picture);

        Models.Picture Delete(Guid pictureId);
    }
}
