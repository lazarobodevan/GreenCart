using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Models;
namespace Tests.Shared.Factories.Picture {
    public class PictureFactory {
        private backend.Models.ProductPicture picture = new() {
            Id = Guid.NewGuid(),
            Position = 0,
            ProductId = Guid.NewGuid(),
        };

        public backend.Models.ProductPicture Build() {
            return picture;
        }

        public PictureFactory WithKey(Guid key) {
            picture.Id = key;
            return this;
        }

        public PictureFactory WithPosition(int pos) {
            picture.Position = pos;
            return this;
        }

        public PictureFactory WithProductId(Guid productId) {
            picture.ProductId = productId;
            return this;
        }

    }
}
