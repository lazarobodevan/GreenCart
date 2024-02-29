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
using backend.ProducerPicture.Services;

namespace backend.Product.UseCases;

public class GetProductByIdUseCase{
    private readonly IProductRepository repository;
    private readonly IProductPictureService pictureService;
    private readonly IProducerPictureService producerPictureService;

    public GetProductByIdUseCase(IProductRepository repository, IProductPictureService pictureService, IProducerPictureService producerPictureService){
        this.repository = repository;
        this.pictureService = pictureService;
        this.producerPictureService = producerPictureService;
    }

    public async Task<ListProductDTO?> Execute(Guid id){

        var productEntity = repository.FindById(id);
        if(productEntity == null) {
            return null;
        }
        //TODO: obter imagens do S3
        List<string> s3Urls = await pictureService.GetImagesAsync(productEntity); 
        List<ListProductPictureDTO> pictures = new List<ListProductPictureDTO>();
        for(int i = 0; i < productEntity.Pictures.Count(); i++) {
            pictures.Add(new ListProductPictureDTO() {
                Position = productEntity.Pictures.ElementAt(i).Position,
                ProductId = productEntity.Pictures.ElementAt(i).ProductId,
                Url = s3Urls.ElementAt(i)
            });
        }
        string? producerPicture = await producerPictureService.GetProfilePictureAsync(productEntity.Producer);
        
        var listProduct = new ListProductDTO(productEntity, pictures, producerPicture);

        return listProduct;
    }
}