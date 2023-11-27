using backend.Models;
using backend.Repositories;

namespace backend.UseCases.Product {
    public class UpdateProductUseCase {
        private readonly IProductRepository repository;

        public UpdateProductUseCase(IProductRepository repository) {
            this.repository = repository;
        }

        public async Task<Models.Product> Execute(Models.Product product) {
            var possibleProduct = await this.repository.FindById(product.Id);
            
            if (possibleProduct != null) {
                product.UpdatedAt = DateTime.Now;
                Models.Product updatedProduct = this.repository.Update(product);

                return updatedProduct;
            }

            throw new Exception("Falha ao atualizar: o produto não existe");
            
        }
    }
}
