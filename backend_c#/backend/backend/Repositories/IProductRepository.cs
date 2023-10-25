using backend.DTOs.Product;
using backend.Models;

namespace backend.Repositories {
    public interface IProductRepository {
        Task<Product> Save(Product product);
        Task<Product?> FindById(Guid productId);
        Task<IEnumerable<Product>> SaveMany(Product[] products);
        Task<Product> Update(Product product);
        Task<Product> Delete(Guid productId);
    }
}
