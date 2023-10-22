using backend.Repositories;

namespace backend.UseCases.Product {
    public class GetProductByIdUseCase {
        private readonly ProductRepository repository;

        public GetProductByIdUseCase(ProductRepository repository) {
            this.repository = repository;
        }

        public async Task<Models.Product?> execute(Guid id) {
            var product = await this.repository.FindById(id);

            return product;
        }
    }
}
