using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Contexts;
using backend.Picture.DTOs;
using backend.Product.DTOs;
using backend.Product.Exceptions;
using backend.Product.Models;
using backend.Utils;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace backend.Product.Repository;

public class ProductRepository : IProductRepository{
    private readonly DatabaseContext _context;

    public ProductRepository(DatabaseContext context){
        _context = context;
    }

    public async Task<backend.Models.Product> Save(backend.Models.Product product, List<CreatePictureDTO> pictures){
        try{
            product.CreatedAt = DateTime.Now;
            product.NormalizedName = new StringUtils().NormalizeString(product.Name);

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

    public async Task<backend.Models.Product?> FindById(Guid productId){
        try{
            var possibleProduct = await _context.Products.Include(p => p.Pictures)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(p => p.Id == productId && p.DeletedAt == null);

            return possibleProduct;
        }
        catch (Exception e){
            throw new Exception("Erro inesperado ao buscar no banco de dados");
        }
    }

    public ListDatabaseProductsPagination GetProducerProducts(Guid producerId, int page, int pageResults, ProductFilterModel? filterModel) {

        var producerExists = _context.Producers.Any<backend.Models.Producer>(producer => producer.Id == producerId && producer.DeletedAt == null);

        if (!producerExists) { 
            throw new ProducerDoesNotExistException();
        }
        IQueryable<backend.Models.Product> filteredQuery;

        //Apply filters if exists
        if(filterModel != null) {
            filteredQuery = _ApplyFilters(_context.Products.AsQueryable(), filterModel);
        } else {
            filteredQuery = _context.Products.AsQueryable();
        }
        
        var productsQuery = filteredQuery
            .Where(product => 
                product.ProducerId == producerId && 
                product.Producer.DeletedAt == null 
                && product.DeletedAt == null
             )
            .Select(product => new backend.Models.Product {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                HarvestDate = product.HarvestDate,
                Unit = product.Unit,
                IsOrganic = product.IsOrganic,
                AvailableQuantity = product.AvailableQuantity,
                Category = product.Category,
                Price = product.Price,
                DeletedAt = product.DeletedAt,
                CreatedAt = product.CreatedAt,
                Pictures = product.Pictures.Where(picture => picture.Position == 0).ToList(),
                ProducerId = producerId,
            })
            .OrderBy(product => product.Name)
            .ToList();

        var totalProductsCount = productsQuery.Count();
        var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageResults);

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

    public async Task<List<backend.Models.Product>> SaveMany(List<backend.Models.Product> products){
        try{
            List<backend.Models.Product> savedProducts = new List<backend.Models.Product>();

            foreach (var product in products){
                product.CreatedAt = DateTime.Now;
                product.NormalizedName = new StringUtils().NormalizeString(product.Name);
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

    public backend.Models.Product Update(backend.Models.Product product){
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
    public async Task<backend.Models.Product> Delete(backend.Models.Product product){
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

    public ListDatabaseProductsPagination FindByFilter(ProductFilterModel filterModel, int page, int pageResults) {
        try {
            var query = _context.Products.AsQueryable();

            query = _ApplyFilters(query, filterModel);

            var totalProductsCount = query.Count();
            var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageResults);
            page = Math.Min(page, (int)pageCount - 1);
            int offset = Math.Max(0, page) * pageResults;

            var products = query
                .Skip(offset)
                .Take((int)pageResults)
                .ToList();

            return new ListDatabaseProductsPagination() {
                CurrentPage = page,
                Products = products,
                Pages = pageCount,
                Offset = offset
            };

        } catch(Exception e) {
            throw new Exception("Erro inesperado ao filtrar produtos");
        }
    }

    private IQueryable<backend.Models.Product> _ApplyFilters(IQueryable<backend.Models.Product> query, ProductFilterModel filterModel) {

        //Find products that are existent. This is due to soft deletion.
        query = query.Where(p => p.DeletedAt == null);

        if (!string.IsNullOrEmpty(filterModel.Name)) {
            string normalizedName = new StringUtils().NormalizeString(filterModel.Name);
            query = query.Where(p => p.NormalizedName.Contains(normalizedName));
        }

        if (filterModel.IsByPrice == true) {
            query = query.OrderBy(p => p.Price);
        }

        if (filterModel.Category != null) {
            query = query.Where(p => p.Category == filterModel.Category);
        }

        if (filterModel.ProducerId != null) {
            query = query.Where(p => p.ProducerId == filterModel.ProducerId);
        }

        if (filterModel.IsOrganic != null) {
            /*
             * If IsOrganic is false, we filter both organic and not organic products.
             */
            if (filterModel.IsOrganic == false) return query;
            query = query.Where(p => p.IsOrganic == filterModel.IsOrganic);
        }

        return query;
    }
}