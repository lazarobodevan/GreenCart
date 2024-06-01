using System;
using System.Threading.Tasks;
using backend.Product.DTOs;
using backend.Product.Enums;
using backend.Product.Repository;
using backend.Utils;

namespace backend.Product.UseCases;

public class UpdateProductUseCase{
    private readonly IProductRepository repository;

    public UpdateProductUseCase(IProductRepository repository){
        this.repository = repository;
    }

    public async Task<backend.Models.Product> Execute(UpdateProductDTO productDTO){
        
        DateTime harvestDate = new DateTime();

        if (productDTO.HarvestDate != null)
            harvestDate = DateUtils.ConvertStringToDateTime(productDTO.HarvestDate, "dd/MM/yyyy");


        var productEntity = new backend.Models.Product{
            Id = productDTO.Id,
            Name = productDTO.Name!,
            AvailableQuantity = (int)productDTO.AvailableQuantity!,
            Category = (Category)productDTO.Category!,
            Description = productDTO.Description,
            Unit = (Unit)productDTO.Unit!,
            HarvestDate = harvestDate,
            IsOrganic = (bool)productDTO.IsOrganic!,
            Price = (double)productDTO.Price!,
            UpdatedAt = DateTime.Now
        };

        var updatedProduct = repository.Update(productEntity);

        return updatedProduct;
    }
}