using backend.DTOs.Producer;
using backend.Models;
using backend.Repositories;

namespace backend.UseCases.Producer {
    public class CreateProducerUseCase {
        private readonly ProducerRepository repository;

        public CreateProducerUseCase(ProducerRepository repository) {
            this.repository = repository;
        }

        public async Task<Models.Producer> Execute(CreateProducerDTO producerDTO) {

            Models.Producer producer = new Models.Producer {
                Name = producerDTO.Name,
                Email = producerDTO.Email,
                Attended_Cities = producerDTO.Attended_Cities,
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
