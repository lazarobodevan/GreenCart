using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Contexts;
using backend.Models;
using backend.Shared.Classes;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

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

    public Pagination<backend.Models.Producer> FindNearProducers(Shared.Classes.Location myLocation, int page, int pageResults) {

        var referenceCoord = new GeoCoordinate(myLocation.Latitude, myLocation.Longitude);

        double radiusInMeters = myLocation.RadiusInKm * 1000;

        var producersQuery = _context.Producers
            .AsEnumerable()
            .Where(producer =>
                referenceCoord.GetDistanceTo(new GeoCoordinate(producer.Latitude, producer.Longitude)) <= radiusInMeters)
            .ToList();

        var totalProductsCount = producersQuery.Count();
        var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageResults);
        page = Math.Min(page, (int)pageCount - 1);

        int offset = Math.Max(0, page) * pageResults;

        var producers = producersQuery
            .Skip(offset)
            .Take((int)pageResults)
            .ToList();

        return new Pagination<backend.Models.Producer>() {
            CurrentPage = page,
            Data = producers,
            Pages = pageCount,
            Offset = offset
        };
    }

    public async Task<Models.Producer> Save(Models.Producer producer){
        producer.CreatedAt = DateTime.Now;
        var createdProducer = await _context.Producers.AddAsync(producer);
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