using backend.Models;

namespace backend.Product.Repository
{
    public interface IProductRepository
    {
        Task<Models.Product> Save(Models.Product product);
        Task<Models.Product?> FindById(Guid productId);
        Task<IEnumerable<Models.Product>> SaveMany(Models.Product[] products);
        Models.Product Update(Models.Product product);
        Task<Models.Product> Delete(Models.Product product);
    }
}
