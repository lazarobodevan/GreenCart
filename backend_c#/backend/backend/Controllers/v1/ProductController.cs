using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Picture.DTOs;
using backend.Producer.Repository;
using backend.Producer.Services;
using backend.Producer.UseCases;
using backend.Product.DTOs;
using backend.Product.Exceptions;
using backend.Product.Repository;
using backend.Product.UseCases;
using backend.Producer.DTOs;
using backend.Utils;
using backend.Utils.Errors;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase{
    private readonly CreateProductUseCase createProductUseCase;
    private readonly GetProducerProductsUseCase getProducerProductsUseCase;
    private readonly FindProducerByIdUseCase getProducerByIdUseCase;
    private readonly IPictureService pictureService;

    private readonly IProductRepository repository;
    private readonly IProducerRepository producerRepository;

    public ProductController(IProductRepository repository, IPictureService _pictureService, IProducerRepository _producerRepository){
        this.repository = repository;
        this.pictureService = _pictureService;
        this.producerRepository = _producerRepository;
        createProductUseCase = new CreateProductUseCase(this.repository, pictureService);
        getProducerProductsUseCase = new GetProducerProductsUseCase(this.repository, pictureService);
        getProducerByIdUseCase = new FindProducerByIdUseCase(this.producerRepository);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDTO productDTO){
        try{
            if (productDTO == null) return BadRequest("Dados incompletos");

            var createdProduct = await createProductUseCase.Execute(productDTO);

            ListProductDTO listCreatedProducts = new ListProductDTO(createdProduct, new List<ListPictureDTO>());

            return StatusCode(StatusCodes.Status201Created, listCreatedProducts);
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

    [HttpGet("producerId={producerId}")]
    public async Task<IActionResult> GetProductsFromProducer(Guid producerId, [FromQuery]int? page) {
        try {
            var resultsPerPage = 5;
            var producer = await getProducerByIdUseCase.Execute(producerId);
            var products = await getProducerProductsUseCase.Execute(producerId, page ?? 0, resultsPerPage);

            return Ok(new ListProductsResponse() {
                CurrentPage = page ?? 0,
                Pages = products.Pages,
                Producer = new ListProducerDTO(producer!),
                Products = products.Products,
                NextUrl = new PaginationUtils().GetNextUrl(page ?? 0, products.Pages, Request.PathBase),
                PreviousUrl = new PaginationUtils().GetPreviousUrl(page ?? 0, Request.PathBase)
            });

        } catch (ProducerDoesNotExistException ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status400BadRequest, error);
        }
        catch(Exception ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}