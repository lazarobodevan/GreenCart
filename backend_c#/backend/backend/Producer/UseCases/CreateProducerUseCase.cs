using backend.Models;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.Utils.Errors;

namespace backend.Producer.UseCases
{
    public class CreateProducerUseCase
    {
        private readonly IProducerRepository repository;

        public CreateProducerUseCase(IProducerRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Models.Producer> Execute(CreateProducerDTO producerDTO)
        {
            Models.Producer? possibleProducer = null;

            try {
                possibleProducer = await repository.FindByEmail(producerDTO.Email);   
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            if (possibleProducer != null) {
                throw new Exception("Usuário já cadastrado");
            }


            try {
                Models.Producer producer = new Models.Producer {
                    Name = producerDTO.Name,
                    Email = producerDTO.Email,
                    AttendedCities = producerDTO.AttendedCities,
                    FavdByConsumers = new List<ConsumerFavProducer>(),
                    CPF = producerDTO.CPF,
                    OriginCity = producerDTO.OriginCity,
                    Password = producerDTO.Password,
                    Telephone = producerDTO.Telephone,
                    WhereToFind = producerDTO.WhereToFind,
                    CreatedAt = DateTime.Now,
                };

                var createdProducer = await repository.Save(producer);

                return createdProducer;

            }catch(Exception e) {
                return null;
            }
        }
    }
}
