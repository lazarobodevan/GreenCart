using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Producer.Repository;
using backend.Product.Repository;

namespace backend.Product.UseCases;

public class GetProducerProductsUseCase{
    private readonly IProductRepository repository;

    public GetProducerProductsUseCase(IProductRepository repository){
        this.repository = repository;
    }

    public async Task<ICollection<Models.Product>> Execute(Guid producerId){
        var possibleProducer = await repository.FindById(producerId);
        if (possibleProducer == null) throw new Exception("Produtor não existe");

        var products = repository.GetProducerProducts(producerId);
        return products;
    }
}