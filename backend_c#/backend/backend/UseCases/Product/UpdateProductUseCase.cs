using backend.Repositories;

namespace backend.UseCases.Product {
    public class UpdateProductUseCase {
        private readonly ProductRepository repository;

        public UpdateProductUseCase(ProductRepository repository) {
            this.repository = repository;
        }

        public async Task<Models.Product> Execute() { }
    }
}
