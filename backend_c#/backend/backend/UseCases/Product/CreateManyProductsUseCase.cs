using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Repositories;
using backend.Utils;
using System.Globalization;
using System.Xml.Linq;

namespace backend.UseCases.Product {
    public class CreateManyProductsUseCase {
        private readonly ProductRepository repository;

        public CreateManyProductsUseCase(ProductRepository repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<Models.Product>> Execute(CreateProductDTO[] productsDTO) {

            DateTime parsedDateTime;
            
            List<Models.Product> productsEntities = new List<Models.Product>();
            foreach (var product in productsDTO) {

                parsedDateTime = DateUtils.ConvertStringToDateTime(product.HarvestDate!, "dd/MM/yyyy");

                Models.Product p = new Models.Product {
                    Name = product.Name,
                    Description = product.Description,
                    Picture = product.Picture,
                    Category = (Category)product.Category!,
                    Price = (double)product.Price!,
                    Unit = (Unit)product.Unit!,
                    AvailableQuantity = (int)product.AvailableQuantity!,
                    IsOrganic = (bool)product.IsOrganic!,
                    HarvestDate = parsedDateTime,
                    ProducerId = (Guid)product.ProducerId!,
                };
                productsEntities.Add(p);
            }

            return await this.repository.SaveMany(productsEntities.ToArray());

        }
    }
}
