using System;
using System.Threading.Tasks;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProducerController : ControllerBase{
    private readonly CreateProducerUseCase createProducerUseCase;

    private readonly IProducerRepository repository;

    public ProducerController(IProducerRepository repository){
        this.repository = repository;
        createProducerUseCase = new CreateProducerUseCase(this.repository);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProducerAsync([FromBody] CreateProducerDTO producerDto){
        if (producerDto == null) return BadRequest("Corpo é obrigatório");

        try{
            var createdProducer = await createProducerUseCase.Execute(producerDto);
            return StatusCode(StatusCodes.Status201Created, new ListProducerDTO(createdProducer));
        }
        catch (Exception ex){
            return StatusCode(StatusCodes.Status500InternalServerError, new{ error = ex.Message });
        }
    }
}