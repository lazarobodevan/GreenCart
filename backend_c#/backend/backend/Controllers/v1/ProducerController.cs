using backend.DTOs.Producer;
using backend.Repositories;
using backend.UseCases.Producer;
using backend.UseCases.Product;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.v1 {
    [ApiController]
    [Route("api/[controller]")]
    public class ProducerController: ControllerBase {

        private readonly ProducerRepository repository;
        private readonly CreateProducerUseCase createProducerUseCase;
        public ProducerController(ProducerRepository repository) {
            this.repository = repository;
            createProducerUseCase = new CreateProducerUseCase(this.repository);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducerAsync([FromBody] CreateProducerDTO producerDto) {
            
            if(producerDto == null) {
                return BadRequest("Corpo é obrigatório");
            }

            try {
                var createdProducer = await createProducerUseCase.Execute(producerDto);
                return StatusCode(StatusCodes.Status201Created, new ListProducerDTO(createdProducer));
            }catch(Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message});
            }
        }
    }
}
