using System;
using System.Threading.Tasks;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.ProducerPicture.DTOs;
using backend.ProducerPicture.Services;

namespace backend.Producer.UseCases;

public class FindProducerByIdUseCase{
    private readonly IProducerRepository _repository;
    private readonly IProducerPictureService _pictureService;

    public FindProducerByIdUseCase(IProducerRepository repository, IProducerPictureService producerPictureService){
        _repository = repository;
        _pictureService = producerPictureService;
    }

    public async Task<ListProducerDTO?> Execute(Guid producerId){
        var possibleProducer = await _repository.FindById(producerId);

        if (possibleProducer == null) return null;

        ListProducerDTO producerDTO = new ListProducerDTO(possibleProducer, null);

        if (possibleProducer.HasProfilePicture) {
            var pictureUrl = await _pictureService.GetProfilePictureAsync(possibleProducer);
            producerDTO.Picture = new ListProducerPictureDTO() {
                ProducerId = possibleProducer.Id,
                Url = pictureUrl.ToString()
            };
        }
        return producerDTO;
    }
}