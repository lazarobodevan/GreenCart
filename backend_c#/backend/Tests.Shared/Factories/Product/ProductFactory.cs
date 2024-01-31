using backend.Models;
using backend.Product.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factories.Product
{
    public class ProductFactory
    {

        private backend.Models.Product ProductEntity = new backend.Models.Product
        {
            Id = Guid.NewGuid(),
            Name = "Product",
            NormalizedName = "PRODUCT",
            Description = "Description",
            Pictures = new List<Picture>(),
            Category = Category.VEGETABLE,
            Price = 10.11,
            Unit = Unit.LITER,
            AvailableQuantity = 1,
            IsOrganic = true,
            HarvestDate = DateTime.Now,
            ProducerId = Guid.NewGuid(),
        };

        public ProductFactory WithId(Guid id)
        {
            ProductEntity.Id = id;
            return this;
        }

        public ProductFactory WithName(string name)
        {
            ProductEntity.Name = name;
            return this;
        }

        public ProductFactory WithDescription(string description)
        {
            ProductEntity.Description = description;
            return this;
        }

        public ProductFactory WithPicture(List<Picture> pictures)
        {
            ProductEntity.Pictures = pictures;
            return this;
        }

        public ProductFactory WithCategory(Category category)
        {
            ProductEntity.Category = category;
            return this;
        }

        public ProductFactory WithPrice(double price)
        {
            ProductEntity.Price = price;
            return this;
        }

        public ProductFactory WithUnit(Unit unit)
        {
            ProductEntity.Unit = unit;
            return this;
        }

        public ProductFactory WithAvailableQuantity(int quantity)
        {
            ProductEntity.AvailableQuantity = quantity;
            return this;
        }

        public ProductFactory WithIsOrganic(bool isOrganic)
        {
            ProductEntity.IsOrganic = isOrganic;
            return this;
        }

        public ProductFactory WithHarvestDate(DateTime date)
        {
            ProductEntity.HarvestDate = date;
            return this;
        }

        public ProductFactory WithProducerId(Guid producerId)
        {
            ProductEntity.ProducerId = producerId;
            return this;
        }

        public backend.Models.Product Build()
        {
            return ProductEntity;
        }
    }
}
