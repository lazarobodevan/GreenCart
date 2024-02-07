using System;
using System.Threading.Tasks;
using backend.Product.Repository;

namespace backend.Product.UseCases;

public class DeleteProductUseCase{
    private readonly IProductRepository repository;

    public DeleteProductUseCase(IProductRepository repository){
        this.repository = repository;
    }

    public async Task<backend.Models.Product> Execute(Guid productId){
        var possibleProduct = repository.FindById(productId);

        if (possibleProduct != null){
            possibleProduct.DeletedAt = DateTime.Now;
            var deletedProduct = repository.Update(possibleProduct);

            return deletedProduct;
        }

        throw new Exception("Falha ao deletar: o produto não existe");
    }
}