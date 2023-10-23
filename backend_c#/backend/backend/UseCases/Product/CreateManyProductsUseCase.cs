using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Repositories;
using System.Xml.Linq;

namespace backend.UseCases.Product {
    public class CreateManyProductsUseCase {
        private readonly ProductRepository repository;

        public CreateManyProductsUseCase(ProductRepository repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<Models.Product>> Execute(CreateProductDTO[] productsDTO) {

            List<Models.Product> productsEntities = new List<Models.Product>();
            foreach (var product in productsDTO) {
                Models.Product p = new Models.Product {
                    Name = product.Name,
                    Description = product.Description,
                    Picture = product.Picture,
                    Category = product.Category,
                    Price = product.Price,
                    Unit = product.Unit,
                    AvailableQuantity = product.AvailableQuantity,
                    IsOrganic = product.IsOrganic,
                    HarvestDate = product.HarvestDate,
                    ProducerId = product.ProducerId,
                };
                productsEntities.Add(p);
            }

            return await this.repository.SaveMany(productsEntities.ToArray());

        }
    }
}
