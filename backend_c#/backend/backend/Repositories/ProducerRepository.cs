using backend.Contexts;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories {
    public class ProducerRepository : IProducerRepository {
        private readonly DatabaseContext _context;
        
        public ProducerRepository(DatabaseContext context) {
            _context = context;
        }

        public async Task<Producer?> FindByEmail(string email) {
            
            var possibleProducer = await this._context.Producers.FirstOrDefaultAsync(producer => producer.Email == email);
            return possibleProducer;

        }

        public async Task<Producer?> FindById(Guid id) {
            return await this._context.Producers.FirstOrDefaultAsync(producer => producer.Id == id);
        }

        public IEnumerable<Producer> FindNearProducers(string city) {
            var producers = this._context.Producers
                .Where(producer => producer.AttendedCities.Contains(city.ToUpper()))
                .Include(producer => producer.Products)
                .ToList();

            return producers;
        }

        public IEnumerable<Product> GetProducts(Guid producerId) {
            //var products = this._context.Producers.Include(producer => producer.Products).SingleOrDefault(producer => producer.Id.Equals(producerId));
            var products = this._context.Producers
                .Where(producer => producer.Id == producerId)
                .SelectMany(producer => producer.Products)
                .ToList();
            
            return products;
        }

        public async Task<Producer> Save(Producer producer) {
            producer.CreatedAt = DateTime.Now;
            var createdProducer = await this._context.Producers.AddAsync(producer);
            await this._context.SaveChangesAsync();
            return createdProducer.Entity;
        }

        public async Task<Producer> Update(Producer producer) {
            producer.UpdatedAt = DateTime.Now;

            var updatedProducer = this._context.Producers.Update(producer);

            await this._context.SaveChangesAsync();

            return updatedProducer.Entity;

        }

        public async Task<Producer> Delete(Producer producer) {
            
            producer.DeletedAt = DateTime.Now;
            
            var deletedProducer = this._context.Producers.Update(producer);
            await this._context.SaveChangesAsync();

            return deletedProducer.Entity;
        }

    }
}
