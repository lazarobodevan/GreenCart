using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using backend.ProducerPicture.DTOs;
using backend.ProducerPicture.Services;
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
        findNearProducersUseCase = new FindNearProducersUseCase(this.repository);
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
    public async Task<IActionResult> GetProducers() {
        try {
            var producers = findNearProducersUseCase.Execute("City");

            List<ListProducerDTO> listProducersDtos = new List<ListProducerDTO>();

            foreach(var producer in producers) { 
                listProducersDtos.Add(new ListProducerDTO(
                    producer, 
                    new ListProducerPictureDTO() { 
                        ProducerId = producer.Id, 
                        Url = ""}
                    )
                );
            }

            return Ok(listProducersDtos);

        }catch (Exception ex) {
            var error = ExceptionUtils.FormatExceptionResponse(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, error);
        }
    }
}