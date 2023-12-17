using backend.Producer.Repository;

namespace backend.Producer.UseCases
{
    public class GetProducerProductsUseCase
    {
        private readonly IProducerRepository repository;

        public GetProducerProductsUseCase(IProducerRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Models.Product>> Execute(Guid producerId)
        {

            var possibleProducer = await repository.FindById(producerId);
            if (possibleProducer == null)
            {
                throw new Exception("Produtor não existe");
            }

            var products = repository.GetProducts(producerId);
            return products;
        }
    }
}
