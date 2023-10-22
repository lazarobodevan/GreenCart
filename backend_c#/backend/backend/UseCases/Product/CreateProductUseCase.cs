using backend.DTOs.Product;
using backend.Models;
using backend.Repositories;

namespace backend.UseCases.Product {
    public class CreateProductUseCase {

        private readonly ProductRepository repository;
        public CreateProductUseCase(ProductRepository _repository) {
            repository = _repository;
        }

        public async Task<Models.Product> execute(CreateProductDTO _productDTO) {

            Models.Product productEntity = new Models.Product {
                Name = _productDTO.Name,
                AvailableQuantity = _productDTO.AvailableQuantity,
                Category = _productDTO.Category,
                HarvestDate = _productDTO.HarvestDate,
                Description = _productDTO.Description,
                IsOrganic = _productDTO.IsOrganic,
                Picture = _productDTO.Picture,
                Price = _productDTO.Price,
                ProducerId = _productDTO.ProducerId,
                Unit = _productDTO.Unit
            };

            var createdProduct = await this.repository.Save(productEntity);

            return createdProduct;
        }

    }
}
