using backend.Contexts;
using backend.Models;

namespace backend.Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Models.Product> Save(Models.Product product)
        {

            product.CreatedAt = DateTime.Now;

            var createdProduct = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return createdProduct.Entity;
        }

        public async Task<Models.Product?> FindById(Guid productId)
        {

            var possibleProduct = await _context.Products.FindAsync(productId);

            return possibleProduct;
        }

        public async Task<IEnumerable<Models.Product>> SaveMany(Models.Product[] products)
        {

            List<Models.Product> savedProducts = new List<Models.Product>();

            foreach (var product in products)
            {
                product.CreatedAt = DateTime.Now;
                var createdProduct = await _context.Products.AddAsync(product);
                savedProducts.Add(createdProduct.Entity);

            }

            await _context.SaveChangesAsync();

            return savedProducts;
        }

        public Models.Product Update(Models.Product product)
        {

            product.UpdatedAt = DateTime.Now;

            var updatedProduct = _context.Products.Update(product);

            return updatedProduct.Entity;

        }

        //Soft delete
        public async Task<Models.Product> Delete(Models.Product product)
        {

            product.DeletedAt = DateTime.Now;

            var deletedProduct = _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return deletedProduct.Entity;

        }
    }
}
