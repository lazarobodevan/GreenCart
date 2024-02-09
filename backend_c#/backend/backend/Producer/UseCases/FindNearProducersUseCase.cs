using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.ProducerPicture.DTOs;
using backend.ProducerPicture.Services;
using backend.Shared.Classes;

namespace backend.Producer.UseCases;

public class FindNearProducersUseCase{
    private readonly IProducerRepository _repository;
    private readonly IProducerPictureService _pictureService;

    public FindNearProducersUseCase(IProducerRepository repository, IProducerPictureService pictureService){
        _repository = repository;
        _pictureService = pictureService;
    }

    public async Task<Pagination<ListProducerDTO>> Execute(Location myLocation, int page, int pageResults){

        var foundProducers = _repository.FindNearProducers(myLocation, page, pageResults);

        var profilePicturesTasks = foundProducers.Data.Select(p => _pictureService.GetProfilePictureAsync(p)).ToArray();
        var profilePictures = await Task.WhenAll(profilePicturesTasks);

        List<ListProducerDTO> listProducerDTOs = new List<ListProducerDTO>();

        for(int i = 0; i < foundProducers.Data.Count(); i++) {

            var currentProducer = foundProducers.Data[i];

            listProducerDTOs.Add(new ListProducerDTO(currentProducer, new ListProducerPictureDTO()));

            if (currentProducer.HasProfilePicture) {
                listProducerDTOs[i].Picture = new ListProducerPictureDTO() {
                    ProducerId = currentProducer.Id,
                    Url = profilePictures[i]!
                };

            }
        }

        return new Pagination<ListProducerDTO>() {
            CurrentPage = page,
            Data = listProducerDTOs,
            Pages = foundProducers.Pages,
        };
    }
}