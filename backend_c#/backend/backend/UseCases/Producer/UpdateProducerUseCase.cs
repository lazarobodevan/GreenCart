using backend.Repositories;

namespace backend.UseCases.Producer {
    public class UpdateProducerUseCase {
        private IProducerRepository repository;

        public UpdateProducerUseCase(IProducerRepository repository) { 
            this.repository = repository;
        }

        public async Task Execute() {

        }
    }
}
