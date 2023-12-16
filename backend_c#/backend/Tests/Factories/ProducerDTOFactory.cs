using backend.DTOs.Producer;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factories {
    public class ProducerDTOFactory {
        
        private CreateProducerDTO createProducerDto = new CreateProducerDTO {
            Name = "Producer Test",
            Email = "test@test.com",
            AttendedCities = "City1;City2;City3",
            CreatedAt = DateTime.Now,
            FavdByConsumers = new List<ConsumerFavProducer>(),
            CPF = "111.111.111-11",
            OriginCity = "City1",
            Password = "123",
            Telephone = "(31) 99999-9999",
            WhereToFind = "Local de encontro"
        };

        public CreateProducerDTO GetDefaultCreateProducerDto() {
            return this.createProducerDto;
        }

        public ProducerDTOFactory WithEmail(string email) {
            this.createProducerDto.Email = email;
            return this;
        }


    }
}
