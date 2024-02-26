using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Contexts;
using backend.Models;
using backend.Producer.Queries;
using backend.Product.Models;
using backend.Shared.Classes;
using backend.Utils;
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
        return await _context.Producers
            .Include(p => p.LocationAddress)
            .FirstOrDefaultAsync(producer => producer.Id == id);
    }

    public Pagination<backend.Models.Producer> FindNearProducers(Shared.Classes.Location myLocation, int page, int pageResults, ProducerFilterQuery? filterQuery) {

        var referenceCoord = new Point(new Coordinate(myLocation.Longitude, myLocation.Latitude));
        List<backend.Models.Producer> producers = new List<Models.Producer>();

        double radiusInMeters = myLocation.RadiusInKm * 1000;

        /*
         * Get nearby producers ordered by:
         * 1 - Distance
         * 2 - Ratings average
         * 3 - Ratings count
         */

        // Filtering by distance
        var nearbyProducers = _context.Producers
            .Where(producer => producer.Location.Distance(referenceCoord) <= radiusInMeters
                && producer.DeletedAt == null);

        // Applying filters
        if (filterQuery != null) {
            nearbyProducers = _ApplyFilters(nearbyProducers.AsQueryable(), filterQuery, referenceCoord);
            producers = nearbyProducers.Include(p => p.LocationAddress).ToList();
        } else {
            //Default order: distance -> ratings count -> ratings avg
            producers = nearbyProducers
            .OrderBy(producer => producer.Location.Distance(referenceCoord))
            .ThenByDescending(producer => producer.RatingsCount)
            .OrderByDescending(producer => producer.RatingsAvg)
            .ToList();
        }

        var totalProductsCount = producers.Count();
        var pageCount = (int)Math.Ceiling((double)totalProductsCount / pageResults);
        page = Math.Min(page, (int)pageCount - 1);

        int offset = Math.Max(0, page) * pageResults;

        var paginatedProducers = producers
            .Skip(offset)
            .Take((int)pageResults)
            .ToList();

        return new Pagination<backend.Models.Producer>() {
            CurrentPage = page,
            Data = paginatedProducers,
            Pages = pageCount,
            Offset = offset
        };
    }

    public async Task<Models.Producer> Save(Models.Producer producer){
        producer.CreatedAt = DateTime.Now;
        producer.NormalizedName = new StringUtils().NormalizeString(producer.Name);

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

    private IQueryable<backend.Models.Producer> _ApplyFilters(IQueryable<backend.Models.Producer> query, ProducerFilterQuery filterModel, NetTopologySuite.Geometries.Point referenceCoord) {

        if (!string.IsNullOrEmpty(filterModel.Name)) {
            string normalizedName = new StringUtils().NormalizeString(filterModel.Name);
            query = query.Where(p => p.NormalizedName.Contains(normalizedName));
        }

        if (filterModel.IsByRating == true) {
            query = query.OrderBy(p => p.RatingsAvg);
        }

        if (filterModel.IsByLocation == true) {
            query = query.OrderBy(producer => producer.Location.Distance(referenceCoord));
        }

        return query;
    }
}