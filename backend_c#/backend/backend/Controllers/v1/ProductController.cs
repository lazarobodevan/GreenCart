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
using backend.Product.Models;
using backend.Product.Enums;
using backend.Shared.Classes;
using backend.ProducerPicture.Services;

namespace backend.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase{

    private readonly CreateProductUseCase createProductUseCase;
    private readonly GetProducerProductsUseCase getProducerProductsUseCase;
    private readonly FindProducerByIdUseCase getProducerByIdUseCase;
    private readonly GetProductByIdUseCase getProductByIdUseCase;

    private readonly IProductPictureService productPictureService;
    private readonly IProducerPictureService producerPictureService;
    private readonly IProductRepository repository;
    private readonly IProducerRepository producerRepository;

    public ProductController(IProductRepository repository, IProductPictureService _pictureService, IProducerRepository _producerRepository, IProducerPictureService producerPictureService){
        this.repository = repository;
        this.productPictureService = _pictureService;
        this.producerRepository = _producerRepository;
        this.producerPictureService = producerPictureService;
        createProductUseCase = new CreateProductUseCase(this.repository, productPictureService);
        getProducerProductsUseCase = new GetProducerProductsUseCase(this.repository, productPictureService);
        getProducerByIdUseCase = new FindProducerByIdUseCase(this.producerRepository, producerPictureService);
        getProductByIdUseCase = new GetProductByIdUseCase(this.repository, productPictureService);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDTO productDTO){
        try{
            if (productDTO == null) return BadRequest("Dados incompletos");

            var createdProduct = await createProductUseCase.Execute(productDTO);

            ListProductDTO listCreatedProducts = new ListProductDTO(createdProduct, new List<ListProductPictureDTO>());

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
    public async Task<IActionResult> GetProductsFromProducer(
        Guid producerId, 
        [FromQuery]int? page, 
        [FromQuery]string? name, 
        [FromQuery]bool? isByPrice, 
        [FromQuery]Category? category, 
        [FromQuery]bool? isOrganic) {


        try {
            ProductFilterModel filterModel = new ProductFilterModel() {
                Name = name,
                Category = category,
                IsByPrice = isByPrice,
                IsOrganic = isOrganic,
            };
            var producer = await getProducerByIdUseCase.Execute(producerId);
            var products = await getProducerProductsUseCase.Execute(producerId, page ?? 0, filterModel);

            return Ok(new ListProductsResponse() {
                
                Producer = producer,
                Products = new Pagination<List<ListProductDTO>>() {
                    CurrentPage = page ?? 0,
                    Pages = products.Pages,
                    NextUrl = new PaginationUtils().GetNextUrl(page ?? 0, products.Pages, Request.PathBase),
                    PreviousUrl = new PaginationUtils().GetPreviousUrl(page ?? 0, Request.PathBase)
                }
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


    [HttpGet("id={productId}")]
    public async Task<IActionResult> GetProductById(Guid productId) {
        try {
            ListProductDTO? productDTO = await getProductByIdUseCase.Execute(productId);
            return Ok(productDTO);
        }catch(Exception ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}