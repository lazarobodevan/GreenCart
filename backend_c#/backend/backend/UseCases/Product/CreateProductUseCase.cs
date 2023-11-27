using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Repositories;
using backend.Utils;

namespace backend.UseCases.Product {
    public class CreateProductUseCase {

        private readonly IProductRepository repository;
        public CreateProductUseCase(IProductRepository _repository) {
            repository = _repository;
        }

        public async Task<Models.Product> Execute(CreateProductDTO _productDTO) {

            DateTime parsedDateTime = DateUtils.ConvertStringToDateTime(_productDTO.HarvestDate!, "dd/MM/yyyy");

            Models.Product productEntity = new Models.Product {
                Name = _productDTO.Name,
                AvailableQuantity = (int)_productDTO.AvailableQuantity!,
                Category = (Category)_productDTO.Category!,
                HarvestDate = parsedDateTime,
                Description = _productDTO.Description,
                IsOrganic = (bool)_productDTO.IsOrganic!,
                Picture = _productDTO.Picture,
                Price = (Double)_productDTO.Price!,
                ProducerId = (Guid)_productDTO.ProducerId!,
                Unit = (Unit)_productDTO.Unit!
            };
            
            var createdProduct = await this.repository.Save(productEntity);
            return createdProduct;
        }

    }
}
