using System;
using System.Threading.Tasks;
using backend.Product.DTOs;
using backend.Product.Exceptions;
using backend.Product.Repository;
using backend.Product.UseCases;
using backend.Utils.Errors;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase{
    private readonly CreateProductUseCase createProductUseCase;

    private readonly IProductRepository repository;

    public ProductController(IProductRepository repository){
        this.repository = repository;
        createProductUseCase = new CreateProductUseCase(this.repository);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDTO productDTO){
        try{
            if (productDTO == null) return BadRequest("Dados incompletos");

            var createdProduct = await createProductUseCase.Execute(productDTO);
            return StatusCode(StatusCodes.Status201Created, new ListProductDTO(createdProduct));
        }
        catch (ProducerDoesNotExistException ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return BadRequest(error);
        }
        catch (Exception ex){
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}