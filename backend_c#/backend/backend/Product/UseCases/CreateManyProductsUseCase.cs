using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Picture.DTOs;
using backend.Product.DTOs;
using backend.Product.Enums;
using backend.Product.Repository;
using backend.Utils;
using Microsoft.AspNetCore.Http;

namespace backend.Product.UseCases;

public class CreateManyProductsUseCase{
    private readonly IProductRepository repository;

    public CreateManyProductsUseCase(IProductRepository repository){
        this.repository = repository;
    }
    
    public async Task<List<backend.Models.Product>> Execute(List<CreateProductDTO> productsDTO){

        DateTime parsedDateTime;

        List<backend.Models.Product> productsEntities = new List<backend.Models.Product>();
        List<List<CreateProductPictureDTO>> createPictureDTOs = new List<List<CreateProductPictureDTO>>();

        foreach (var product in productsDTO){
            
            parsedDateTime = DateUtils.ConvertStringToDateTime(product.HarvestDate!, "dd/MM/yyyy");
            var productId = Guid.NewGuid();
            var p = new backend.Models.Product{
                Id = productId,
                Name = product.Name,
                Description = product.Description,
                Category = (Category)product.Category!,
                Price = (double)product.Price!,
                Unit = (Unit)product.Unit!,
                AvailableQuantity = (int)product.AvailableQuantity!,
                IsOrganic = (bool)product.IsOrganic!,
                HarvestDate = parsedDateTime,
                ProducerId = (Guid)product.ProducerId!
            };
            productsEntities.Add(p);
            createPictureDTOs.Add(_SaveImages(product.Pictures!, productId));
        }

        var savedProducts = await repository.SaveMany(productsEntities);
        return savedProducts;
    }


    private List<CreateProductPictureDTO> _SaveImages(List<IFormFile> pictures, Guid productId) {
        //TODO: Padronizar o nome dos arquivos para fazer salvamento em massa de varios
        //produtos com varias imagens
        //TODO: Adicionar validação do nome do arquivo
        
        //Na criação de um novo produto, so pode fazer upload de uma imagem
        List<CreateProductPictureDTO> picturesEntity = new List<CreateProductPictureDTO>();

        foreach (var picture in pictures) {
            picturesEntity.Add(new CreateProductPictureDTO() {
                Key = Guid.NewGuid(),
                Stream = picture.OpenReadStream(),
                Position = 1
            });
        }

        return picturesEntity;
    }
}