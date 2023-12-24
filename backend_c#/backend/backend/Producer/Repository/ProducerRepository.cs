using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Contexts;
using Microsoft.EntityFrameworkCore;

namespace backend.Producer.Repository;

public class ProducerRepository : IProducerRepository{
    private readonly DatabaseContext _context;

    public ProducerRepository(DatabaseContext context){
        _context = context;
    }

    public async Task<Models.Producer?> FindByEmail(string email){
        try{
            var possibleProducer = await _context.Producers.FirstOrDefaultAsync(producer => producer.Email == email);
            return possibleProducer;
        }
        catch (Exception e){
            return null;
        }
    }

    public async Task<Models.Producer?> FindById(Guid id){
        return await _context.Producers.FirstOrDefaultAsync(producer => producer.Id == id);
    }

    public IEnumerable<Models.Producer> FindNearProducers(string city){
        var producers = _context.Producers
            .Where(producer => producer.AttendedCities.Contains(city.ToUpper()))
            .Include(producer => producer.Products)
            .ToList();

        return producers;
    }

    public IEnumerable<Models.Product> GetProducts(Guid producerId){
        //var products = this._context.Producers.Include(producer => producer.Products).SingleOrDefault(producer => producer.Id.Equals(producerId));
        var products = _context.Producers
            .Where(producer => producer.Id == producerId)
            .SelectMany(producer => producer.Products)
            .ToList();

        return products;
    }

    public async Task<Models.Producer> Save(Models.Producer producer){
        producer.CreatedAt = DateTime.Now;
        Console.WriteLine("*********************************************************************************");
        Console.WriteLine(producer.Name);
        var createdProducer = await _context.Producers.AddAsync(producer);
        Console.WriteLine(createdProducer.Entity.Id);
        await _context.SaveChangesAsync();
        return createdProducer.Entity;
    }

    public async Task<Models.Producer> Update(Models.Producer producer){
        producer.UpdatedAt = DateTime.Now;

        var updatedProducer = _context.Producers.Update(producer);

        await _context.SaveChangesAsync();

        return updatedProducer.Entity;
    }

    public async Task<Models.Producer> Delete(Models.Producer producer){
        producer.DeletedAt = DateTime.Now;

        var deletedProducer = _context.Producers.Update(producer);
        await _context.SaveChangesAsync();

        return deletedProducer.Entity;
    }
}