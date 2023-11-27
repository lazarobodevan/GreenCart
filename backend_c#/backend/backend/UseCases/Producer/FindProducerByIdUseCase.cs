using backend.Models;
using backend.Repositories;

namespace backend.UseCases.Producer {
    public class FindProducerByIdUseCase {
        private readonly IProducerRepository _repository;

        public FindProducerByIdUseCase(IProducerRepository repository) {
            _repository = repository;
        }

        public async Task<Models.Producer?> Execute(Guid producerId) {

            var possibleProducer = await _repository.FindById(producerId);

            return possibleProducer;

        }
    }
}
