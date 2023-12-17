using backend.Models;
using backend.Exceptions;
using backend.Producer.Repository;
using backend.Producer.DTOs;

namespace backend.Producer.UseCases
{
    public class UpdateProducerUseCase
    {
        private IProducerRepository repository;

        public UpdateProducerUseCase(IProducerRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Models.Producer> Execute(UpdateProducerDTO updateProducerDTO)
        {

            var possibleProducer = await repository.FindById(updateProducerDTO.Id);

            if (possibleProducer == null)
            {
                throw new Exception("Produtor não existe");
            }

            Models.Producer producerEntity = new Models.Producer
            {
                Id = updateProducerDTO.Id,
                Name = updateProducerDTO.Name ?? possibleProducer.Name,
                AttendedCities = updateProducerDTO.AttendedCities ?? possibleProducer.AttendedCities,
                CPF = updateProducerDTO.CPF ?? possibleProducer.CPF,
                Email = updateProducerDTO.Email ?? possibleProducer.Email,
                Password = updateProducerDTO.Password ?? possibleProducer.Password,
                OriginCity = updateProducerDTO.OriginCity ?? possibleProducer.OriginCity,
                Picture = updateProducerDTO.Picture ?? possibleProducer.Picture,
                Telephone = updateProducerDTO.Telephone ?? possibleProducer.Telephone,
                WhereToFind = updateProducerDTO.WhereToFind ?? possibleProducer.WhereToFind,
                UpdatedAt = DateTime.Now
            };

            var updatedProducer = repository.Update(producerEntity);

            return updatedProducer.Result;
        }
    }
}
