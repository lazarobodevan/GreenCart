using backend.Product.DTOs;
using backend.Product.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factories.Product
{
    public class ProductDTOFactory
    {

        private CreateProductDTO createProductDto = new CreateProductDTO
        {
            Name = "Product test",
            Description = "Description",
            AvailableQuantity = 10,
            Category = Category.BEVERAGE,
            HarvestDate = "25/11/2023",
            IsOrganic = true,
            Pictures = new List<IFormFile>(),
            Price = 10.50,
            Unit = Unit.LITER,
            ProducerId = Guid.NewGuid(),
        };


        public CreateProductDTO Build()
        {
            return createProductDto;
        }

        public ProductDTOFactory WithProducerId(Guid producerId) {
            createProductDto.ProducerId = producerId;
            return this;
        }

        public ProductDTOFactory WithName(string name)
        {
            createProductDto.Name = name;
            return this;
        }

        public ProductDTOFactory WithDescription(string description)
        {
            createProductDto.Description = description;
            return this;
        }

        public ProductDTOFactory WithCategory(Category category)
        {
            createProductDto.Category = category;
            return this;
        }

        public ProductDTOFactory WithHarvestDate(string harvestDate)
        {
            createProductDto.HarvestDate = harvestDate;
            return this;
        }

        public ProductDTOFactory WithIsOrganic(bool isOrganic)
        {
            createProductDto.IsOrganic = isOrganic;
            return this;
        }

        public ProductDTOFactory WithPictures(List<IFormFile> pictures)
        {
            createProductDto.Pictures = pictures;
            return this;
        }
        public ProductDTOFactory WithPrice(double price)
        {
            createProductDto.Price = price;
            return this;
        }

        public ProductDTOFactory WithUnit(Unit unit)
        {
            createProductDto.Unit = unit;
            return this;
        }

    }
}
