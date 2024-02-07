using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using backend.ProducerPicture.DTOs;
using backend.Utils.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProducerController : ControllerBase{
    private readonly CreateProducerUseCase createProducerUseCase;
    private readonly FindNearProducersUseCase findNearProducersUseCase;

    private readonly IProducerRepository repository;

    public ProducerController(IProducerRepository repository){
        this.repository = repository;
        createProducerUseCase = new CreateProducerUseCase(this.repository);
        findNearProducersUseCase = new FindNearProducersUseCase(this.repository);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProducerAsync([FromBody] CreateProducerDTO producerDto){
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