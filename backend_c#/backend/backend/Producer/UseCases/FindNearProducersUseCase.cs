﻿using backend.Producer.Repository;

namespace backend.Producer.UseCases
{
    public class FindNearProducersUseCase
    {
        private readonly IProducerRepository _repository;

        public FindNearProducersUseCase(IProducerRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Models.Producer> Execute(string city)
        {

            var foundProducers = _repository.FindNearProducers(city);

            return foundProducers;
        }
    }
}