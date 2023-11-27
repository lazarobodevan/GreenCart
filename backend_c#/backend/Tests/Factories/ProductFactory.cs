using backend.DTOs.Product;
using backend.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factories {
    public class ProductFactory {
        
        private CreateProductDTO createProductDto = new CreateProductDTO {
            Name = "Product test",
            Description = "Description",
            AvailableQuantity = 10,
            Category = backend.Enums.Category.BEVERAGE,
            HarvestDate = "25/11/2023",
            IsOrganic = true,
            Picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
            Price = 10.50,
            Unit = backend.Enums.Unit.LITER,
    };

        public ProductFactory GetDefaultCreateProductDto(Guid producerId) {
            createProductDto.ProducerId = producerId;
            return this;
        }

        public CreateProductDTO Build() {
            return this.createProductDto;
        }

        public ProductFactory WithName(string name) {
            this.createProductDto.Name = name;
            return this;
        }

        public ProductFactory WithDescription(string description) {
            this.createProductDto.Description = description;
            return this;
        }

        public ProductFactory WithCategory(Category category) {
            this.createProductDto.Category = category;
            return this;
        }

        public ProductFactory WithHarvestDate(string harvestDate) {
            this.createProductDto.HarvestDate = harvestDate;
            return this;
        }

        public ProductFactory WithIsOrganic(bool isOrganic) {
            this.createProductDto.IsOrganic = isOrganic;
            return this;
        }

        public ProductFactory WithPicture(byte[] picture) {
            this.createProductDto.Picture = picture;
            return this;
        }
        public ProductFactory WithPrice(double price) {
            this.createProductDto.Price = price;
            return this;
        }

        public ProductFactory WithUnit(Unit unit) {
            this.createProductDto.Unit = unit;
            return this;
        }

    }
}
