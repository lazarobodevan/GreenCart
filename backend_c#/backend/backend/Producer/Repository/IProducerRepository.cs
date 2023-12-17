using backend.Models;

namespace backend.Producer.Repository
{
    public interface IProducerRepository
    {
        Task<Models.Producer> Save(Models.Producer producer);
        Task<Models.Producer?> FindById(Guid id);
        Task<Models.Producer?> FindByEmail(string email);
        IEnumerable<Models.Product> GetProducts(Guid producerId);
        IEnumerable<Models.Producer> FindNearProducers(string city);
        Task<Models.Producer> Update(Models.Producer producer);
        Task<Models.Producer> Delete(Models.Producer producer);
    }
}
