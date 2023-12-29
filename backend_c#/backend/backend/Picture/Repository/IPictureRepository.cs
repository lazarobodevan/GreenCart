using backend.Picture.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Picture.Repository {
    public interface IPictureRepository {
        Task<List<Models.Picture>> Create(List<CreatePictureDTO> pictures, Guid productId, Guid producerId);
    }
}
