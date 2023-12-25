using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Product.Repository;

public interface IProductRepository{
    Task<Models.Product> Save(Models.Product product);
    Task<Models.Product?> FindById(Guid productId);
    ICollection<Models.Product> GetProducerProducts(Guid producerId);
    Task<IEnumerable<Models.Product>> SaveMany(Models.Product[] products);
    Models.Product Update(Models.Product product);
    Task<Models.Product> Delete(Models.Product product);
}