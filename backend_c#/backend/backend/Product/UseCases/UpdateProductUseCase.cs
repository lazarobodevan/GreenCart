using backend.Models;
using backend.Product.DTOs;
using backend.Product.Repository;
using backend.Utils;

namespace backend.Product.UseCases
{
    public class UpdateProductUseCase
    {
        private readonly IProductRepository repository;

        public UpdateProductUseCase(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Models.Product> Execute(UpdateProductDTO productDTO)
        {

            var possibleProduct = await repository.FindById(productDTO.Id);
            DateTime harvestDate = new DateTime();

            if (possibleProduct == null)
            {
                throw new Exception("Produto não existe");
            }

            if (productDTO.HarvestDate != null)
            {
                harvestDate = DateUtils.ConvertStringToDateTime(productDTO.HarvestDate, "dd/MM/yyyy");
            }


            var productEntity = new Models.Product
            {
                Id = possibleProduct.Id,
                Name = productDTO.Name ?? possibleProduct.Name,
                AvailableQuantity = productDTO.AvailableQuantity ?? possibleProduct.AvailableQuantity,
                Category = productDTO.Category ?? possibleProduct.Category,
                Description = productDTO.Description ?? possibleProduct.Description,
                Unit = productDTO.Unit ?? possibleProduct.Unit,
                HarvestDate = harvestDate != DateTime.MinValue ? harvestDate : possibleProduct.HarvestDate,
                IsOrganic = productDTO.IsOrganic ?? possibleProduct.IsOrganic,
                Picture = productDTO.Picture ?? possibleProduct.Picture,
                Price = productDTO.Price ?? possibleProduct.Price,
                UpdatedAt = DateTime.Now,
            };

            var updatedProduct = repository.Update(productEntity);

            return updatedProduct;

        }
    }
}
