using backend.Picture.DTOs;
using backend.Product.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Product.Repository;

public interface IProductRepository{
    Task<Models.Product> Save(Models.Product product, List<CreatePictureDTO> pictures);
    Task<Models.Product?> FindById(Guid productId);
    ListDatabaseProductsPagination GetProducerProducts(Guid producerId, int page, int pageResults);
    Task<List<Models.Product>> SaveMany(List<Models.Product> products);
    Models.Product Update(Models.Product product);
    Task<Models.Product> Delete(Models.Product product);
}