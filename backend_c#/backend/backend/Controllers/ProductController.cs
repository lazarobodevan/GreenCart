using backend.DTOs.Product;
using backend.Repositories;
using backend.UseCases.Product;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {
    public class ProductController : Controller {

        private readonly ProductRepository repository;
        private readonly CreateProductUseCase createProductUseCase;
        public ProductController(ProductRepository repository) {
            this.repository = repository;
            this.createProductUseCase = new CreateProductUseCase(this.repository);
        }

        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDTO productDTO) {

            try {
                if (productDTO == null) {
                    return BadRequest("Dados incompletos");
                }

                var createdProduct = await this.createProductUseCase.execute(productDTO);
                return View(new ListProductDTO(createdProduct));

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
