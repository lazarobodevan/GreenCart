using backend.DTOs.Producer;
using backend.Models;
using backend.Repositories;
using backend.Utils.Errors;

namespace backend.UseCases.Producer {
    public class CreateProducerUseCase {
        private readonly ProducerRepository repository;

        public CreateProducerUseCase(ProducerRepository repository) {
            this.repository = repository;
        }

        public async Task<Models.Producer> Execute(CreateProducerDTO producerDTO) {

            var possibleProducer = await repository.FindByEmail(producerDTO.Email);

            if(possibleProducer != null) {
                throw new Exception("Usuário já cadastrado");
            }

            Models.Producer producer = new Models.Producer {
                Name = producerDTO.Name,
                Email = producerDTO.Email,
                AttendedCities = producerDTO.AttendedCities,
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = producerDTO.CPF,
                OriginCity = producerDTO.OriginCity,
                Password = producerDTO.Password,
                Telephone = producerDTO.Telephone,
                WhereToFind = producerDTO.WhereToFind
            };

            var createdProducer = await this.repository.Save(producer);

            return createdProducer;
        }
    }
}
