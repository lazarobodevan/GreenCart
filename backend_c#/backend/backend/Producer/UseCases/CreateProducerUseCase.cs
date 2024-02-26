using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using backend.Models;
using backend.Producer.DTOs;
using backend.Producer.Repository;
using backend.ProducerPicture.DTOs;
using backend.ProducerPicture.Services;
using backend.Shared.Services.Location;
using NetTopologySuite;

namespace backend.Producer.UseCases;

public class CreateProducerUseCase{
    private readonly IProducerRepository repository;
    private readonly IProducerPictureService producerPictureService;
    private readonly ILocationService locationService;

    public CreateProducerUseCase(IProducerRepository repository, IProducerPictureService producerPictureService, ILocationService locationService){
        this.repository = repository;
        this.producerPictureService = producerPictureService;
        this.locationService = locationService;
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

        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326); // SRID para WGS84
        var locationPoint = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate((double)producerDTO.Longitude!, (double)producerDTO.Latitude!));
        var locationAdress = locationService.GetLocationByLatLon((double)producerDTO.Latitude!, (double)producerDTO.Longitude!);

        var producer = new Models.Producer {
            Name = producerDTO.Name,
            Email = producerDTO.Email,
            FavdByConsumers = new List<ConsumerFavProducer>(),
            WhereToFind = producerDTO.WhereToFind,
            LocationAddress = locationAdress,
            HasProfilePicture = producerDTO.Picture != null,
            Password = producerDTO.Password,
            Telephone = producerDTO.Telephone,
            Location = locationPoint,
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