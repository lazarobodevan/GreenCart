using backend.Repositories;

namespace backend.UseCases.Producer {
    public class GetProducerProductsUseCase {
        private readonly ProducerRepository repository;

        public GetProducerProductsUseCase(ProducerRepository repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<Models.Product>> Execute(Guid producerId) {

            var possibleProducer = await this.repository.FindById(producerId);
            if (possibleProducer == null) {
                throw new Exception("Produtor não existe");
            }

            var products = this.repository.GetProducts(producerId);
            return products;
        }
    }
}
