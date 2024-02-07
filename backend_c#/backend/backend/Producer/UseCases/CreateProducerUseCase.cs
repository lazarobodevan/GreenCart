using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using backend.Models;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.ProducerPicture.DTOs;
using backend.ProducerPicture.Services;

namespace backend.Producer.UseCases;

public class CreateProducerUseCase{
    private readonly IProducerRepository repository;
    private readonly IProducerPictureService producerPictureService;

    public CreateProducerUseCase(IProducerRepository repository, IProducerPictureService producerPictureService){
        this.repository = repository;
        this.producerPictureService = producerPictureService;
    }

    public async Task<Models.Producer> Execute(CreateProducerDTO producerDTO){
        Models.Producer? possibleProducer = null;

        try{
            possibleProducer = await repository.FindByEmail(producerDTO.Email);
        }
        catch (Exception ex){
            Console.WriteLine(ex.Message);
        }

        if (possibleProducer != null) throw new Exception("Usuário já cadastrado");

        var producer = new Models.Producer {
            Name = producerDTO.Name,
            Email = producerDTO.Email,
            AttendedCities = producerDTO.AttendedCities,
            FavdByConsumers = new List<ConsumerFavProducer>(),
            HasProfilePicture = producerDTO.Picture != null,
            OriginCity = producerDTO.OriginCity,
            Password = producerDTO.Password,
            Telephone = producerDTO.Telephone,
            WhereToFind = producerDTO.WhereToFind,
            CreatedAt = DateTime.Now
        };

        var createdProducer = await repository.Save(producer);

        if (producerDTO.Picture != null) {
            var picture = new CreateProducerPictureDTO() {
                Key = createdProducer.Id,
                Stream = producerDTO.Picture.OpenReadStream(),
            };
            var createdPicture = await producerPictureService.UploadProfilePictureAsync(createdProducer, picture);
        }

        return createdProducer;
        
    }
}