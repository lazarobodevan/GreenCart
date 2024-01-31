using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Product.DTOs;
using backend.Product.Repository;
using backend.Product.Exceptions;
using backend.Picture.DTOs;
using backend.Producer.Services;
using System.Linq;
using backend.Models;

namespace backend.Product.UseCases;

public class GetProductByIdUseCase{
    private readonly IProductRepository repository;
    private readonly IPictureService pictureService;

    public GetProductByIdUseCase(IProductRepository repository, IPictureService pictureService){
        this.repository = repository;
        this.pictureService = pictureService;
    }

    public async Task<ListProductDTO?> Execute(Guid id){

        var productEntity = await repository.FindById(id);
        if(productEntity == null) {
            return null;
        }
        //TODO: obter imagens do S3
        List<string> s3Urls = await pictureService.GetImagesAsync(productEntity); 
        List<ListPictureDTO> pictures = new List<ListPictureDTO>();
        for(int i = 0; i < productEntity.Pictures.Count(); i++) {
            pictures.Add(new ListPictureDTO() {
                Position = productEntity.Pictures.ElementAt(i).Position,
                ProductId = productEntity.Pictures.ElementAt(i).ProductId,
                Url = s3Urls.ElementAt(i)
            });
        }
        var listProduct = new ListProductDTO(productEntity, pictures);

        return listProduct;
    }
}