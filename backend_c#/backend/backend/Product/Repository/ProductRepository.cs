using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Contexts;
using backend.Picture.DTOs;
using backend.Product.DTOs;
using backend.Product.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Repository;

public class ProductRepository : IProductRepository{
    private readonly DatabaseContext _context;

    public ProductRepository(DatabaseContext context){
        _context = context;
    }

    public async Task<Models.Product> Save(Models.Product product, List<CreatePictureDTO> pictures){
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

    public ListDatabaseProductsPagination GetProducerProducts(Guid producerId, int page, int pageResults) {

        var producerExists = _context.Producers.Any<Models.Producer>(producer => producer.Id == producerId && producer.DeletedAt == null);

        if (!producerExists) { 
            throw new ProducerDoesNotExistException();
        }

        var productsQuery = _context.Producers
            .Where(producer => producer.Id == producerId && producer.DeletedAt == null)
            .SelectMany(products => products.Products)
            .Include(pictures => pictures.Pictures.Where(picture => picture.Position == 0))
            .OrderBy(product => product.Name);

        var totalProductsCount = productsQuery.Count();
        var pageCount = totalProductsCount / pageResults;

        page = Math.Min(page, (int)pageCount-1);

        int offset = Math.Max(0, page) * pageResults;

        var products = productsQuery
            .Skip(offset)
            .Take((int)pageResults)
            .ToList();

        return new ListDatabaseProductsPagination() {
            CurrentPage = page,
            Products = products,
            Pages = pageCount,
            Offset = offset
        };
    }

    public async Task<List<Models.Product>> SaveMany(List<Models.Product> products){
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
        catch(ReferenceConstraintException ex) {
            throw new ProducerDoesNotExistException();
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