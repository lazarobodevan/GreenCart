using backend.Picture.DTOs;
using backend.Producer.DTOs;
using backend.Producer.Queries;
using backend.Producer.Services;
using backend.ProducerPicture.DTOs;
using backend.Product.DTOs;
using backend.Product.Models;
using backend.Product.Repository;
using backend.Shared.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Product.UseCases {
    public class GetNearbyProductsUseCase {
        private readonly IProductRepository _repository;
        private readonly IProductPictureService _pictureService;

        public GetNearbyProductsUseCase(IProductRepository repository, IProductPictureService productPictureService) {
            this._repository = repository;
            this._pictureService = productPictureService;
        }

        public async Task<Pagination<ListProductDTO>> Execute(Location myLocation, int page, int pageResults, ProductFilterQuery? filterQuery) {

            var foundProducts = _repository.FindNearProducts(myLocation, page, pageResults, filterQuery);

            var profilePicturesTasks = foundProducts.Data.Select(p => _pictureService.GetImagesAsync(p)).ToArray();
            var profilePictures = await Task.WhenAll(profilePicturesTasks);

            List<ListProductDTO> listProductsDTOs = new List<ListProductDTO>();
            

            for (int i = 0; i < foundProducts.Data.Count(); i++) {

                var currentProduct = foundProducts.Data[i];
                List<ListProductPictureDTO> productPictures = new List<ListProductPictureDTO>();

                productPictures.Add(new ListProductPictureDTO() {
                    Position = currentProduct.Pictures[0].Position,
                    ProductId = currentProduct.Id,
                    Url = profilePictures[i][0]
                });

                listProductsDTOs.Add(new ListProductDTO(currentProduct, productPictures));

            }

            return new Pagination<ListProductDTO>() {
                CurrentPage = page,
                Data = listProductsDTOs,
                Pages = foundProducts.Pages,
            };
        }
    }
}
