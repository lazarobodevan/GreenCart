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
            
            product.CreatedAt = DateTime.Now;

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
                product.CreatedAt = DateTime.Now;
                var createdProduct = await _context.Products.AddAsync(product);
                savedProducts.Add(createdProduct.Entity);

            }

            await _context.SaveChangesAsync();

            return savedProducts;
        }

        public Product Update(Product product) {

            product.UpdatedAt = DateTime.Now;
            
            var updatedProduct = this._context.Products.Update(product);

            return updatedProduct.Entity;
            
        }

        //Soft delete
        public Product Delete(Product product) {
            
            product.DeletedAt = DateTime.Now;

            var deletedProduct = this.Update(product);

            return deletedProduct;
           
        }
    }
}
