using backend.Models;

namespace backend.Repositories {
    public interface IProducerRepository {
        Task<Producer> Save(Producer producer);
        Task<Producer?> FindById(Guid id);
        Task<Producer?> FindByEmail(string email);
        IEnumerable<Product> GetProducts(Guid producerId);
        IEnumerable<Producer> GetNearProducers(string city);
    }
}
