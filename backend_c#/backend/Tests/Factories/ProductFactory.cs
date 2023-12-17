using backend.Models;
using backend.Product.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factories
{
    internal class ProductFactory {

        private Product ProductEntity = new Product {
            Id = Guid.NewGuid(),
            Name = "Product",
            Description = "Description",
            Picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
            Category = Category.VEGETABLE,
            Price = 10.11,
            Unit = Unit.LITER,
            AvailableQuantity = 1,
            IsOrganic = true,
            HarvestDate = DateTime.Now,
            ProducerId = Guid.NewGuid(),
        };

        public ProductFactory WithId(Guid id) {
            ProductEntity.Id = id;
            return this;
        }

        public ProductFactory WithName(string name) {
            ProductEntity.Name = name;
            return this;
        }

        public ProductFactory WithDescription(string description) {
            ProductEntity.Description = description;
            return this;
        }

        public ProductFactory WithPicture(byte[] picture) { 
            ProductEntity.Picture = picture; 
            return this; 
        }

        public ProductFactory WithCategory(Category category) { 
            ProductEntity.Category = category; 
            return this; 
        }

        public ProductFactory WithPrice(double price) { 
            ProductEntity.Price = price; 
            return this; 
        }

        public ProductFactory WithUnit(Unit unit) { 
            ProductEntity.Unit = unit; 
            return this; 
        }

        public ProductFactory WithAvailableQuantity(int quantity) { 
            ProductEntity.AvailableQuantity = quantity; 
            return this; 
        }

        public ProductFactory WithIsOrganic(bool isOrganic) { 
            ProductEntity.IsOrganic = isOrganic; 
            return this; 
        }

        public ProductFactory WithHarvestDate(DateTime date) { 
            ProductEntity.HarvestDate = date; 
            return this; 
        }

        public ProductFactory WithProducerId(Guid producerId) { 
            ProductEntity.ProducerId = producerId; 
            return this; 
        }

        public Product Build() {
            return ProductEntity;
        }
    }
}
