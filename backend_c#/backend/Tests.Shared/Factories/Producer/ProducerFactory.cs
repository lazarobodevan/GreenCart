using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factories.Producer
{
    public class ProducerFactory
    {

        private backend.Models.Producer ProducerEntity = new backend.Models.Producer {
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

        public ProducerFactory WithId(Guid id)
        {
            ProducerEntity.Id = id;
            return this;
        }

        public ProducerFactory WithName(string name)
        {
            ProducerEntity.Name = name;
            return this;
        }

        public ProducerFactory WithEmail(string email)
        {
            ProducerEntity.Email = email;
            return this;
        }

        public ProducerFactory WithAttendedCities(string attendedCities)
        {
            ProducerEntity.AttendedCities = attendedCities;
            return this;
        }

        public ProducerFactory WithFavdByConsumers(List<ConsumerFavProducer> favd)
        {
            ProducerEntity.FavdByConsumers = favd;
            return this;
        }

        public ProducerFactory WithCPF(string CPF)
        {
            ProducerEntity.CPF = CPF;
            return this;
        }

        public ProducerFactory WithOriginCity(string originCity)
        {
            ProducerEntity.OriginCity = originCity;
            return this;
        }

        public ProducerFactory WithPassword(string password)
        {
            ProducerEntity.Password = password;
            return this;
        }

        public ProducerFactory WithTelephone(string telephone)
        {
            ProducerEntity.Telephone = telephone;
            return this;
        }

        public ProducerFactory WithWhereToFind(string whereToFind)
        {
            ProducerEntity.WhereToFind = whereToFind;
            return this;
        }

        public backend.Models.Producer Build() {
            return ProducerEntity;
        }
    }
}
