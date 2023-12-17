using backend.Repositories;

namespace backend.UseCases.Producer {
    public class DeleteProducerUseCase {

        private readonly IProducerRepository _repository;

        public DeleteProducerUseCase(IProducerRepository repository) {
            _repository = repository;
        }

        public async Task<Models.Producer> Execute(Guid producerId) {
            var possibleProducer = await _repository.FindById(producerId);

            if(possibleProducer == null) {
                throw new Exception("Produtor não existe");
            }

            var deletedProducer = await _repository.Delete(possibleProducer);

            return deletedProducer;
        }

    }
}
