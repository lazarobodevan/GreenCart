using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Producer.DTOs;
using backend.Producer.Queries;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using backend.ProducerPicture.DTOs;
using backend.ProducerPicture.Services;
using backend.Shared.Classes;
using backend.Utils;
using backend.Utils.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProducerController : ControllerBase{
    private readonly CreateProducerUseCase createProducerUseCase;
    private readonly FindNearProducersUseCase findNearProducersUseCase;
    private readonly FindProducerByIdUseCase findProducerByIdUseCase;

    private readonly IProducerRepository repository;
    private readonly IProducerPictureService producerPictureService;

    public ProducerController(IProducerRepository repository, IProducerPictureService producerPictureService){
        this.repository = repository;
        this.producerPictureService = producerPictureService;
        createProducerUseCase = new CreateProducerUseCase(this.repository, this.producerPictureService);
        findNearProducersUseCase = new FindNearProducersUseCase(this.repository, this.producerPictureService);
        findProducerByIdUseCase = new FindProducerByIdUseCase(this.repository, producerPictureService);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProducerAsync([FromForm] CreateProducerDTO producerDto){
        if (producerDto == null) return BadRequest("Corpo é obrigatório");

        try{
            var createdProducer = await createProducerUseCase.Execute(producerDto);
            return StatusCode(StatusCodes.Status201Created, new ListProducerDTO(createdProducer, null));
        }
        catch (Exception ex){
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProducerById([FromRoute] Guid id) {
        try {
            var foundProducer = await findProducerByIdUseCase.Execute(id);

            return Ok(foundProducer);
        }catch (Exception ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetProducersNearby(
        [FromQuery] ProducerSearchNearbyQuery searchParameters,
        [FromQuery] ProducerFilterQuery? filterQuery,
        [FromQuery] int? page
        ) {
        try {
            int resultsPerPage = 10;

            var producersPagination = await findNearProducersUseCase.Execute(
                new Shared.Classes.Location() {
                    Latitude = (double)searchParameters.Latitude!,
                    Longitude = (double)searchParameters.Longitude!,
                    RadiusInKm = (int)searchParameters.RadiusInKm!
                },
                page ?? 0,
                resultsPerPage,
                filterQuery
            );

            producersPagination.NextUrl = new PaginationUtils().GetNextUrl(page ?? 0, producersPagination.Pages, Request.PathBase);
            producersPagination.PreviousUrl = new PaginationUtils().GetPreviousUrl(page ?? 0, Request.PathBase);

            return Ok(producersPagination);

        }catch (Exception ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}