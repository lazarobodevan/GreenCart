using backend.Picture.DTOs;
using backend.Picture.Exceptions;
using backend.Picture.Repository;
using backend.Picture.UseCases;
using backend.Producer.Services;
using backend.Product.Exceptions;
using backend.Product.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.ProductPicture.UseCases {
    public class UpdateProductPictureUseCase {
        private readonly IProductPictureService _productPictureService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IProductRepository _productRepository;
        private readonly CreatePicturesUseCase _createPicturesUseCase;

        public UpdateProductPictureUseCase(IProductPictureService productPictureService, IPictureRepository pictureRepository, IProductRepository productRepository) {
            _productPictureService = productPictureService;
            _pictureRepository = pictureRepository;
            _productRepository = productRepository;
            _createPicturesUseCase = new CreatePicturesUseCase(_pictureRepository, _productPictureService, _productRepository);
        }

        public async Task<List<Models.ProductPicture>?> Execute(List<CreateProductPictureDTO>? newPictures, List<ListProductPictureDTO>? productPictures, Guid productId) {
            try {
                var possibleProduct = _productRepository.FindById(productId);
                if(possibleProduct == null) { throw new ProductDoesNotExistException(); }


                if(newPictures != null && newPictures.Count > 0) {
                    var createdPictures = await _createPicturesUseCase.Execute(newPictures, possibleProduct.Id);
                    await _productPictureService.UploadImageAsync(newPictures, possibleProduct);
                }

                List<Models.ProductPicture> productPictureModelToUpdate = new List<Models.ProductPicture>();
                if(productPictures != null && productPictures.Count > 0) {

                    var isPicturesToReorderExists = true;
                    foreach (var p in productPictures) {
                        isPicturesToReorderExists = possibleProduct.Pictures.Find(prod => prod.Id == p.ProductId) != null;
                        if(!isPicturesToReorderExists) { throw new PictureDoesNotExistException(); }
                    }
                     

                    foreach(var productPicture in productPictures) {
                        productPictureModelToUpdate.Add(new Models.ProductPicture() {
                            Position = productPicture.Position,
                            ProductId = possibleProduct.Id,
                        });
                    }
                }


                var updatedPictures = _pictureRepository.Update(productPictureModelToUpdate);

                return updatedPictures;

            }catch (Exception ex) {
                throw new Exception($"Erro ao atualizar imagens do produto {productId}: ${ex.Message}");
            }
        }
    }
}
