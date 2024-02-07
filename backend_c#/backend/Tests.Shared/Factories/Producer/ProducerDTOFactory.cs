using backend.Models;
using backend.Producer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factories.Producer
{
    public class ProducerDTOFactory
    {

        private CreateProducerDTO createProducerDto = new CreateProducerDTO
        {
            Name = "Producer Test",
            Email = "test@test.com",
            AttendedCities = "City1;City2;City3",
            FavdByConsumers = new List<ConsumerFavProducer>(),
            OriginCity = "City1",
            Password = "123",
            Telephone = "(31) 99999-9999",
            WhereToFind = "Local de encontro"
        };

        public CreateProducerDTO Build() {
            return createProducerDto;
        }
        public ProducerDTOFactory WithName(string name) {
            createProducerDto.Name = name;
            return this;
        }
        
        public ProducerDTOFactory WithEmail(string email)
        {
            createProducerDto.Email = email;
            return this;
        }

        public ProducerDTOFactory WithAttendedCities(string attendedCities) {
            createProducerDto.AttendedCities = attendedCities;
            return this;
        }

        public ProducerDTOFactory WithFavdByConsumers(List<ConsumerFavProducer> favs) {
            createProducerDto.FavdByConsumers = favs;
            return this;
        }

        public ProducerDTOFactory WithOriginCity(string originCity) {
            createProducerDto.OriginCity = originCity;
            return this;
        }

        public ProducerDTOFactory WithPassword(string cpf) {
            createProducerDto.Password = cpf;
            return this;
        }

        public ProducerDTOFactory WithTelephone(string telephone) {
            createProducerDto.Telephone = telephone;
            return this;
        }

        public ProducerDTOFactory WithWhereToFind(string whereToFind) {
            createProducerDto.WhereToFind = whereToFind;
            return this;
        }


    }
}
