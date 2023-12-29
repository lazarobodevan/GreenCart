using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Picture.DTOs;
using backend.Producer.Repository;
using backend.Producer.Services;
using backend.Product.DTOs;
using backend.Product.Repository;

namespace backend.Product.UseCases;

public class GetProducerProductsUseCase{
    private readonly IProductRepository repository;
    private readonly IPictureService pictureService;

    public GetProducerProductsUseCase(IProductRepository repository, IPictureService pictureService) {
        this.repository = repository;
        this.pictureService = pictureService;
    }

    public async Task<ListProductsPagination> Execute(Guid producerId, int page, int pageResults){

        var productsPaginated = repository.GetProducerProducts(producerId, page, pageResults);
        
        List<ListProductDTO> listProductsDto = new List<ListProductDTO>();

        foreach (var product in productsPaginated.Products) {

            List<ListPictureDTO> productPicturesUrls = new List<ListPictureDTO>();

            var url = await pictureService.GetImagesAsync(product);
            productPicturesUrls.Add(new ListPictureDTO() {
                ProductId = product.Id,
                Url = url.ElementAt(0) ?? "",
            });

            listProductsDto.Add(new ListProductDTO(product, productPicturesUrls));
        }
        return new ListProductsPagination() {
            Products = listProductsDto,
            CurrentPage = productsPaginated.CurrentPage,
            Offset = productsPaginated.Offset,
            Pages = productsPaginated.Pages
        };
    }
}