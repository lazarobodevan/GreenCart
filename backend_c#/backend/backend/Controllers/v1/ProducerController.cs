using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.Producer.UseCases;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducerController: ControllerBase {

        private readonly IProducerRepository repository;
        private readonly CreateProducerUseCase createProducerUseCase;
        public ProducerController(IProducerRepository repository) {
            this.repository = repository;
            createProducerUseCase = new CreateProducerUseCase(this.repository);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducerAsync([FromBody] CreateProducerDTO producerDto) {
            
            if(producerDto == null) {
                return BadRequest("Corpo é obrigatório");
            }

            try {
                Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                var createdProducer = await createProducerUseCase.Execute(producerDto);
                Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                return StatusCode(StatusCodes.Status201Created, new ListProducerDTO(createdProducer));
            }catch(Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message});
            }
        }
    }
}
