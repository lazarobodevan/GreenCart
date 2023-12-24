using backend.Models;
using backend.Producer.DTOs;
using backend.Product.DTOs;
using backend.Product.Enums;
using backend.Utils;
using FluentAssertions;
using FluentAssertions.Extensions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using backend.Exceptions;
using Tests.Factories.Producer;
using Tests.Factories.Product;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Tests.IntegrationTests.v1
{
    public class ProductControllerTest:IAsyncLifetime, IClassFixture<EnvironmentConfiguration> {

        private EnvironmentConfiguration _environmentConfiguration;

        public async Task DisposeAsync() {
            await _environmentConfiguration.DisposeAsync();
        }

        public async Task InitializeAsync() {
            await _environmentConfiguration.ConfigureTestDatabase();
        }

        public ProductControllerTest() {
            _environmentConfiguration = new EnvironmentConfiguration();
        }

        [Fact]
        public async Task Create_GivenProducerAndProduct_ReturnsListProductsDTO() {
            //Arrange
            var productClient = new RestClient("http://localhost:5212/api/Product");
            var producerClient = new RestClient("http://localhost:5212/api/Producer");

            var productRequest = new RestRequest();
            var producerRequest = new RestRequest();

            var productDTO = new ProductDTOFactory().Build();
            var producerDTO = new ProducerDTOFactory().Build();

            //Act
            producerRequest.Method = Method.Post;
            producerRequest.AddJsonBody(JsonConvert.SerializeObject(producerDTO));
            producerRequest.AddHeader("Content-Type", "application/json");

            RestResponse producerResponse = await producerClient.ExecuteAsync(producerRequest);

            var producerResponseObj = JsonConvert.DeserializeObject<ListProducerDTO>(producerResponse.Content);

            productDTO.ProducerId = producerResponseObj?.Id;

            productRequest.Method = Method.Post;
            productRequest.AddJsonBody(JsonConvert.SerializeObject(productDTO));
            productRequest.AddHeader("Content-Type", "application/json");

            RestResponse productResponse = await productClient.ExecuteAsync(productRequest);
            var productResponseObj = JsonConvert.DeserializeObject<ListProductDTO>(productResponse.Content);

            //Assert
            Assert.True(productResponse.IsSuccessful);
            Assert.True(producerResponse.IsSuccessful);

            producerResponseObj.Should().BeEquivalentTo(producerDTO, options => options.ExcludingMissingMembers());
            productResponseObj.Should().BeEquivalentTo(productDTO, options => options.ExcludingMissingMembers().Excluding(dto => dto.HarvestDate));

            Assert.IsType<DateTime>(productResponseObj!.HarvestDate);
            Assert.Equal(productResponseObj.HarvestDate, DateUtils.ConvertStringToDateTime(productDTO.HarvestDate!, "dd/MM/yyyy"));
        }

        [Fact]
        public async Task Create_GivenProductAndNotExistentProducer_ReturnsException() {
            //Arrange
            var productClient = new RestClient("http://localhost:5212/api/Product");

            var productRequest = new RestRequest();

            var productDTO = new ProductDTOFactory()
                .WithProducerId(Guid.NewGuid())
                .Build();

            productRequest.Method = Method.Post;
            productRequest.AddJsonBody(JsonConvert.SerializeObject(productDTO));
            productRequest.AddHeader("Content-Type", "application/json");

            //Act
            RestResponse productResponse = await productClient.ExecuteAsync(productRequest);
            ExceptionResponseModel productResponseObj =
                JsonConvert.DeserializeObject<ExceptionResponseModel>(productResponse.Content);
            
            Assert.False(productResponse.IsSuccessful);
            Assert.Equal("Produtor não existe", productResponseObj.Error.Message);
            Assert.Equal(HttpStatusCode.BadRequest, productResponse.StatusCode);
            

        }

        [Fact]
        public async Task ShouldThrowErrorOfMissingFieldsInRequestBody() {
            
            //Arrange
            var producerClient = new RestClient("http://localhost:5212/api/Producer");
            var producerRequest = new RestRequest();

            JsonDocument expectedResult = JsonDocument.Parse("{\"CPF\":[\"CPF é obrigatório\"],\"Name\":[\"Nome é obrigatório\"],\"Email\":[\"Email é obrigatório\"],\"Password\":[\"Senha é obrigatória\"],\"Telephone\":[\"Celular é obrigatório\"],\"OriginCity\":[\"Cidade de origem é obrigatório\"],\"WhereToFind\":[\"Onde te encontrar é obrigatório\"],\"AttendedCities\":[\"Cidades atendidas é obrigatório\"]}");

            //Act
            producerRequest.Method = Method.Post;
            producerRequest.AddJsonBody(JsonConvert.SerializeObject(new { }));
            producerRequest.AddHeader("Content-Type", "application/json");

            RestResponse producerResponse = await producerClient.ExecuteAsync(producerRequest);

            var responseObj = JsonConvert.DeserializeObject<ApiResponse>(producerResponse.Content);

            //Assert
            string expectedJson = expectedResult.RootElement.ToString();
            string actualJson = JsonConvert.SerializeObject(responseObj.Errors);
            Assert.Equal(expectedJson, actualJson);

        }

    }
}

