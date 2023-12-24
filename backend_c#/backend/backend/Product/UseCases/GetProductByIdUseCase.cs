using System;
using System.Threading.Tasks;
using backend.Product.Repository;

namespace backend.Product.UseCases;

public class GetProductByIdUseCase{
    private readonly IProductRepository repository;

    public GetProductByIdUseCase(IProductRepository repository){
        this.repository = repository;
    }

    public async Task<Models.Product?> Execute(Guid id){
        var product = await repository.FindById(id);

        return product;
    }
}