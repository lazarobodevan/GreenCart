using backend.Picture.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Shared.Factories.Picture {
    public class CreatePictureDTOFactory {

        private CreatePictureDTO _pictureDTO = new CreatePictureDTO() {
            Key = Guid.NewGuid(),
            Position = 0,
            Stream = new MemoryStream()
        };

        public CreatePictureDTO Build() {
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

        public CreatePictureDTOFactory WithStream(MemoryStream stream) {
            _pictureDTO.Stream = stream;
            return this;
        }
    }
}
