using backend.Contexts;
using backend.Picture.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Picture.Repository {
    public class PictureRepository : IPictureRepository {

        private readonly DatabaseContext _context;

        public PictureRepository(DatabaseContext context) {
            _context = context;
        }

        public async Task<List<Models.Picture>> Create(List<CreatePictureDTO> pictures, Guid productId, Guid producerId) {
            try {
                List<Models.Picture> createdPicturesMetadata = new List<Models.Picture>();
                foreach (var picture in pictures) {
                   var created = await _context.Pictures.AddAsync(new Models.Picture() {
                        Key = picture.Key,
                        Position = picture.Position,
                        ProductId = productId
                    });

                    createdPicturesMetadata.Add(created.Entity);
                }
                await _context.SaveChangesAsync();

                return createdPicturesMetadata;
            }catch(Exception e) {
                throw new Exception("Falha ao salvar imagem no banco de dados");
            }
        }
    }
}
