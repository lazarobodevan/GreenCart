using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Contexts;
using backend.Product.Exceptions;
using EntityFramework.Exceptions.Common;

namespace backend.Product.Repository;

public class ProductRepository : IProductRepository{
    private readonly DatabaseContext _context;

    public ProductRepository(DatabaseContext context){
        _context = context;
    }

    public async Task<Models.Product> Save(Models.Product product){
        try{
            product.CreatedAt = DateTime.Now;

            var createdProduct = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return createdProduct.Entity;
        }
        catch (ReferenceConstraintException ex){
            throw new ProducerDoesNotExistException();
        }
        catch (Exception e){
            throw new Exception("Erro inesperado ao salvar no banco de dados");
        }
    }

    public async Task<Models.Product?> FindById(Guid productId){
        try{
            var possibleProduct = await _context.Products.FindAsync(productId);

            return possibleProduct;
        }
        catch (Exception e){
            throw new Exception("Erro inesperado ao buscar no banco de dados");
        }
    }

    public ICollection<Models.Product> GetProducerProducts(Guid producerId) {
        //var products = this._context.Producers.Include(producer => producer.Products).SingleOrDefault(producer => producer.Id.Equals(producerId));
        var products = _context.Producers
            .Where(producer => producer.Id == producerId)
            .SelectMany(producer => producer.Products)
            .ToList();

        return products;
    }

    public async Task<IEnumerable<Models.Product>> SaveMany(Models.Product[] products){
        try{
            List<Models.Product> savedProducts = new List<Models.Product>();

            foreach (var product in products){
                product.CreatedAt = DateTime.Now;
                var createdProduct = await _context.Products.AddAsync(product);
                savedProducts.Add(createdProduct.Entity);
            }

            await _context.SaveChangesAsync();

            return savedProducts;
        }
        catch (Exception e){
            throw new Exception("Erro inesperado ao salvar no banco de dados");
        }
    }

    public Models.Product Update(Models.Product product){
        try{
            product.UpdatedAt = DateTime.Now;

            var updatedProduct = _context.Products.Update(product);

            return updatedProduct.Entity;
        }
        catch (Exception e){
            throw new Exception("Erro inesperado ao atualizar no banco de dados");
        }
    }

    //Soft delete
    public async Task<Models.Product> Delete(Models.Product product){
        try{
            product.DeletedAt = DateTime.Now;

            var deletedProduct = _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return deletedProduct.Entity;
        }
        catch (Exception e){
            throw new Exception("Erro inesperado ao deletar no banco de dados");
        }
    }
}