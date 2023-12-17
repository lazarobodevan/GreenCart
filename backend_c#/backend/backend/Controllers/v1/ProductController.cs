using backend.Product.DTOs;
using backend.Product.Repository;
using backend.Product.UseCases;
using backend.Utils.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace backend.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository repository;
        private readonly CreateProductUseCase createProductUseCase;
        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
            createProductUseCase = new CreateProductUseCase(this.repository);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDTO productDTO)
        {

            try
            {
                if (productDTO == null)
                {
                    return BadRequest("Dados incompletos");
                }

                var createdProduct = await createProductUseCase.Execute(productDTO);
                return StatusCode(StatusCodes.Status201Created, new ListProductDTO(createdProduct));

            }
            catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorUtils.FormatError(ex)) ;
            }
        }
    }
}
