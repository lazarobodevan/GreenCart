using backend.Repositories;

namespace backend.UseCases.Producer {
    public class GetProducerProductsUseCase {
        private readonly ProducerRepository repository;

        public GetProducerProductsUseCase(ProducerRepository repository) {
            this.repository = repository;
        }

        public IEnumerable<Models.Product> Execute(Guid producerId) {
            var products = this.repository.GetProducts(producerId);
            return products;
        }
    }
}
