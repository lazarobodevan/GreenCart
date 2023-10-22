using backend.Contexts;
using backend.DTOs.Product;
using backend.Models;

namespace backend.Repositories {
    public class ProductRepository : IProductRepository{
        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext context) {
            _context = context;
        }

        public async Task<Product> Save(Product product) {

            var createdProduct = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return createdProduct.Entity;
        }

        public async Task<Product?> FindById(Guid productId) {

            var possibleProduct = await _context.Products.FindAsync(productId);

            return possibleProduct;
        }

        public async Task<IEnumerable<Product>> SaveMany(Product[] products) {

            List<Product> savedProducts = new List<Product>();

            foreach(var product in  products) {

                var createdProduct = await _context.Products.AddAsync(product);
                savedProducts.Add(createdProduct.Entity);

            }

            await _context.SaveChangesAsync();

            return savedProducts;
        }
    }
}
