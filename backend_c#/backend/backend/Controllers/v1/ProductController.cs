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
using backend.Shared.Queries;
using backend.ProductPicture.UseCases;
using backend.Picture.Repository;

namespace backend.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase{

    private readonly CreateProductUseCase createProductUseCase;
    private readonly GetProducerProductsUseCase getProducerProductsUseCase;
    private readonly FindProducerByIdUseCase getProducerByIdUseCase;
    private readonly GetProductByIdUseCase getProductByIdUseCase;
    private readonly GetNearbyProductsUseCase getNearbyProductsUseCase;
    private readonly UpdateProductPictureUseCase updateProductPictureUseCase;
    private readonly UpdateProductUseCase updateProductUseCase;

    private readonly IProductPictureService productPictureService;
    private readonly IProducerPictureService producerPictureService;
    private readonly IProductRepository repository;
    private readonly IProducerRepository producerRepository;
    private readonly IPictureRepository pictureRepository;

    public ProductController(
        IProductRepository repository, 
        IProductPictureService _pictureService, 
        IProducerRepository _producerRepository,
        IProducerPictureService producerPictureService,
        IPictureRepository pictureRepository){

        this.repository = repository;
        this.productPictureService = _pictureService;
        this.producerRepository = _producerRepository;
        this.producerPictureService = producerPictureService;
        this.pictureRepository = pictureRepository;

        createProductUseCase = new CreateProductUseCase(this.repository, productPictureService);
        getProducerProductsUseCase = new GetProducerProductsUseCase(this.repository, productPictureService);
        getProducerByIdUseCase = new FindProducerByIdUseCase(this.producerRepository, producerPictureService);
        getProductByIdUseCase = new GetProductByIdUseCase(this.repository, productPictureService, producerPictureService);
        getNearbyProductsUseCase = new GetNearbyProductsUseCase(this.repository, this.productPictureService);
        updateProductPictureUseCase = new UpdateProductPictureUseCase(this.productPictureService, this.pictureRepository, this.repository);
        updateProductUseCase = new UpdateProductUseCase(this.repository);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDTO productDTO){
        try{
            if (productDTO == null) return BadRequest("Dados incompletos");

            var createdProduct = await createProductUseCase.Execute(productDTO);

            ListProductDTO listCreatedProducts = new ListProductDTO(createdProduct, new List<ListProductPictureDTO>(), null);

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
            ProductFilterQuery filterModel = new ProductFilterQuery() {
                Name = name,
                Category = category,
                IsByPrice = isByPrice,
                IsOrganic = isOrganic,
            };
            var producer = await getProducerByIdUseCase.Execute(producerId);
            var products = await getProducerProductsUseCase.Execute(producerId, page ?? 0, filterModel);

            return Ok(new ListProductsResponse() {
                
                Producer = producer,
                Products = new Pagination<ListProductDTO>() {
                    CurrentPage = page ?? 0,
                    Pages = products.Pages,
                    Data = products.Data,
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

    [HttpGet()]
    public async Task<IActionResult> GetNearbyProducts(
        [FromQuery] SearchNearbyQuery query,
        [FromQuery] ProductFilterQuery? filterQuery,
        [FromQuery] int? page
        ) {
        try {
            var nearbyProducts = await getNearbyProductsUseCase.Execute(new Shared.Classes.Location() {
                Latitude = (double)query.Latitude!,
                Longitude = (double)query.Longitude!,
                RadiusInKm = (int)query.RadiusInKm!

            }, 
            page ?? 0, 
            10, 
            filterQuery);

            return Ok(nearbyProducts);

        }catch(Exception ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
    
    [HttpPut()]
    public async Task<IActionResult> UpdateProduct(UpdateProductDTO newProduct) {
        try {
            var updatedProduct = await updateProductUseCase.Execute(newProduct);
            return Ok(new UpdateProductDTO() { 
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                AvailableQuantity = updatedProduct.AvailableQuantity,
                Category = updatedProduct.Category,
                Description = updatedProduct.Description,
                HarvestDate = DateUtils.ConvertDateTimeToString(updatedProduct.HarvestDate),
                IsOrganic = updatedProduct.IsOrganic,
                Price = updatedProduct.Price,
                Unit = updatedProduct.Unit
            });

        } catch (Exception e) {
            var error = ExceptionUtils.FormatExceptionResponse(e);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }

    }
}