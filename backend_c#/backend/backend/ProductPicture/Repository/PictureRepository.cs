using backend.Contexts;
using backend.Picture.DTOs;
using backend.Picture.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Picture.Repository {
    public class PictureRepository : IPictureRepository {

        private readonly DatabaseContext _context;

        public PictureRepository(DatabaseContext context) {
            _context = context;
        }

        public async Task<List<Models.ProductPicture>> Create(List<Models.ProductPicture> pictures) {
            try {
                List<Models.ProductPicture> createdPicturesMetadata = new List<Models.ProductPicture>();
                foreach (var picture in pictures) {
                   var created = await _context.Pictures.AddAsync(new Models.ProductPicture() {
                        Id = picture.Id,
                        Position = picture.Position,
                        ProductId = picture.ProductId
                    });

                    createdPicturesMetadata.Add(created.Entity);
                }
                await _context.SaveChangesAsync();

                return createdPicturesMetadata;
            }catch(Exception e) {
                throw new Exception("Falha ao salvar imagem no banco de dados");
            }
        }

        public List<Models.ProductPicture> Update(List<Models.ProductPicture> newPictures) {
            try {

                /*
                 * Test if new pictures names match the stored ones.
                 * Picture keys must never change.
                 */
                var updatedPictures = new List<Models.ProductPicture>();
                var productId = newPictures.ElementAt(0).ProductId;
                var productPictures = _context.Pictures.Where(p => p.ProductId == productId).ToList();
                foreach (var picture in newPictures) {
                    var foundPicture = productPictures.Find(p => p.Id == picture.Id);
                    if (foundPicture == null) {
                        throw new PictureDoesNotExistException();
                    }
                }
                //Swap positions
                foreach (var newPicture in newPictures) {

                    var storedPicture = productPictures.Find(p => p.Id == newPicture.Id);

                    //If stored position is different from the new, then swap
                    if (storedPicture!.Position != newPicture.Position) {
                        var updated = _MoveImage(productPictures, newPicture);
                        updatedPictures = updated;
                    }
                }
                _context.SaveChanges();
                return updatedPictures.OrderBy(p => p.Position).ToList();                

            } catch (Exception ex) {
                throw new Exception($"Falha ao atualizar imagem no banco de dados: {ex.Message}");
            }
        }


        public Models.ProductPicture Delete(Guid pictureKey) {
            try {
                var picture = _context.Pictures.Find(pictureKey);
                if (picture == null) {
                    throw new PictureDoesNotExistException();
                }

                var deletedPicture = _context.Pictures.Remove(picture);
                _context.SaveChanges();
                return deletedPicture.Entity;


            } catch (Exception ex) {
                throw new Exception($"Falha ao deletar a imagem do banco de dados: {ex.Message}");
            }
        }

        public List<Models.ProductPicture> FindPicturesFromProduct(Guid productId) {
            try {
                var pictures = _context.Pictures
                    .Where(p => p.ProductId == productId)
                    .OrderBy(p => p.Position)
                    .ToList();

                return pictures;
            } catch (Exception ex) {
                throw new Exception($"Falha ao buscar imagens do produto: {ex.Message}");
            }
        }

        private List<backend.Models.ProductPicture> _MoveImage(List<Models.ProductPicture> productPictures, Models.ProductPicture toUpdatePicture) {

            try {
                var currentStoredPicture = productPictures.Find(p => p.Id == toUpdatePicture.Id);
                var currentPosition = currentStoredPicture!.Position;

                var pictureToSwap = productPictures.Find(p => p.Position == toUpdatePicture.Position);
                var positionToSwap = pictureToSwap!.Position;

                currentStoredPicture.Position = positionToSwap;
                pictureToSwap.Position = currentPosition;
                
                return productPictures;
            }catch (Exception ex) {
                throw new Exception($"Falha ao mover imagens: {ex.Message}");
            }
        }
    }
}
