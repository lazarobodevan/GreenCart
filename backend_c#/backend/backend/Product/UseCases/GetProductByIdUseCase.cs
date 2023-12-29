using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Product.DTOs;
using backend.Product.Repository;
using backend.Product.Exceptions;
using backend.Picture.DTOs;

namespace backend.Product.UseCases;

public class GetProductByIdUseCase{
    private readonly IProductRepository repository;

    public GetProductByIdUseCase(IProductRepository repository){
        this.repository = repository;
    }

    public async Task<ListProductDTO?> Execute(Guid id){

        var productEntity = await repository.FindById(id);
        if(productEntity == null) {
            return null;
        }
        //TODO: obter imagens do S3
        List<ListPictureDTO> picturesUrls = new List<ListPictureDTO>();
        var listProduct = new ListProductDTO(productEntity, picturesUrls);

        return listProduct;
    }
}