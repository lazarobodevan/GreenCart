using backend.Shared.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Producer.Repository;

public interface IProducerRepository{
    Task<Models.Producer> Save(Models.Producer producer);
    Task<Models.Producer?> FindById(Guid id);
    Task<Models.Producer?> FindByEmail(string email);
    Pagination<Models.Producer> FindNearProducers(Location myLocation, int page, int pageResults);
    Task<Models.Producer> Update(Models.Producer producer);
    Task<Models.Producer> Delete(Models.Producer producer);
}