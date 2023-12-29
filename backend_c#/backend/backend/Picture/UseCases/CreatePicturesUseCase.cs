using backend.Picture.DTOs;
using backend.Picture.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Picture.UseCases {
    public class CreatePicturesUseCase {
        private readonly IPictureRepository _pictureRepository;

        public CreatePicturesUseCase(IPictureRepository pictureRepository) {
            _pictureRepository = pictureRepository;
        }

       
    }
}
