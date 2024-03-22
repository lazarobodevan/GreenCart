using backend.Picture.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Shared.Factories.Picture {
    public class CreatePictureDTOFactory {

        private CreateProductPictureDTO _pictureDTO = new CreateProductPictureDTO() {
            Key = Guid.NewGuid(),
            Position = 0,
            Stream = new FormFile(Stream.Null,0,0,"","")
        };

        public CreateProductPictureDTO Build() {
            return _pictureDTO;
        }

        public CreatePictureDTOFactory WithKey(Guid key) {
            _pictureDTO.Key = key;
            return this;
        }

        public CreatePictureDTOFactory WithPosition(int position) {
            _pictureDTO.Position = position;
            return this;
        }

        public CreatePictureDTOFactory WithStream(FormFile stream) {
            _pictureDTO.Stream = stream;
            return this;
        }
    }
}
