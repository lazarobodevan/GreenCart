using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using backend.Picture.DTOs;
using backend.Producer.Repository;
using backend.Producer.Services;
using backend.Product.DTOs;
using backend.Product.Models;
using backend.Product.Repository;
using backend.Shared.Classes;

namespace backend.Product.UseCases;

public class GetProducerProductsUseCase{
    private readonly IProductRepository repository;
    private readonly IProductPictureService pictureService;

    public GetProducerProductsUseCase(IProductRepository repository, IProductPictureService pictureService) {
        this.repository = repository;
        this.pictureService = pictureService;
    }

    public async Task<Pagination<ListProductDTO>> Execute(Guid producerId, int page, ProductFilterModel filterModel){

        var pageResults = 10;

        var productsPaginated = repository.GetProducerProducts(producerId, page, pageResults, _HasAtLeastOneFilterOption(filterModel) ? filterModel : null);
        
        List<ListProductDTO> listProductsDto = new List<ListProductDTO>();

        foreach (var product in productsPaginated.Data) {

            List<ListProductPictureDTO> productPicturesUrls = new List<ListProductPictureDTO>();

            var url = await pictureService.GetImagesAsync(product);
            productPicturesUrls.Add(new ListProductPictureDTO() {
                ProductId = product.Id,
                Url = url.ElementAt(0) ?? "",
            });

            listProductsDto.Add(new ListProductDTO(product, productPicturesUrls));
        }
        return new Pagination<ListProductDTO>() {
            Data = listProductsDto,
            CurrentPage = productsPaginated.CurrentPage,
            Offset = productsPaginated.Offset,
            Pages = productsPaginated.Pages
        };
    }

    private static bool _HasAtLeastOneFilterOption(ProductFilterModel filterModel) {
        if(filterModel == null) return false;

        Type type = typeof(ProductFilterModel);
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties) {
            object? value = property.GetValue(filterModel);

            if(value != null) {
                return true;
            }
        }

        return false;
    }
}