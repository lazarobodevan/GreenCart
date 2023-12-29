using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Picture.DTOs;
using backend.Producer.Services;
using backend.Product.DTOs;
using backend.Product.Enums;
using backend.Product.Repository;
using backend.Utils;
using Microsoft.AspNetCore.Http;

namespace backend.Product.UseCases;

public class CreateProductUseCase{
    private readonly IProductRepository repository;
    private readonly IPictureService pictureService;

    public CreateProductUseCase(IProductRepository _repository, IPictureService _pictureService){
        repository = _repository;
        pictureService = _pictureService;
    }

    public async Task<Models.Product> Execute(CreateProductDTO _productDTO){
        
        DateTime parsedDateTime = DateUtils.ConvertStringToDateTime(_productDTO.HarvestDate!, "dd/MM/yyyy");
        var productId = Guid.NewGuid();
        var productEntity = new Models.Product{
            Id = productId,
            Name = _productDTO.Name,
            Pictures = new List<Models.Picture>(),
            AvailableQuantity = (int)_productDTO.AvailableQuantity!,
            Category = (Category)_productDTO.Category!,
            HarvestDate = parsedDateTime,
            Description = _productDTO.Description,
            IsOrganic = (bool)_productDTO.IsOrganic!,
            Price = (double)_productDTO.Price!,
            ProducerId = (Guid)_productDTO.ProducerId!,
            Unit = (Unit)_productDTO.Unit!
        };

        
        var picturesList = _GeneratePicturesEntity(_productDTO.Pictures!, _productDTO.PicturesMetadata!);
        foreach (var picture in picturesList) {
            productEntity.Pictures.Add(new Models.Picture() {
                Key = picture.Key,
                Position = picture.Position,
                ProductId = productEntity.Id
            });
        }
        await pictureService.UploadImageAsync(picturesList, productEntity);

        //TODO: Adicionar salvamento no repositório
        var createdProduct = await repository.Save(productEntity, picturesList);
        return createdProduct;
    }

    private List<CreatePictureDTO> _GeneratePicturesEntity(List<IFormFile> pictures, List<PictureRequestDTO> metadata) {

        List<CreatePictureDTO> pictureList = new List<CreatePictureDTO>();

        foreach (var picture in pictures) {

            var position = metadata.Find(m => m.Name == picture.FileName)?.Position;
            if(position == null) {
                throw new Exception($"Metadado faltante para imagem {picture.Name}");
            }

            pictureList.Add(new CreatePictureDTO() {
                Key = Guid.NewGuid(),
                Position = (int)position,
                Stream = picture.OpenReadStream(),
            });
        }

        return pictureList;
    }

    private bool _CheckIfValidFileName(string strPosition) {
        int result;
        if(int.TryParse(strPosition, out result) && result >= 0) {
            return true;
        }
        throw new Exception($"Nome inválido do arquivo ${strPosition}");
    }
}