using backend.DTOs.Producer;
using backend.DTOs.Product;
using backend.Enums;
using backend.Models;
using backend.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Tests.IntegrationTests.v1 {
    public class ProductControllerTest: IAsyncLifetime {

        public async Task DisposeAsync() {
            EnvironmentConfiguration configuration = new EnvironmentConfiguration();
            await configuration.DisposeAsync();
        }

        public async Task InitializeAsync() {
            EnvironmentConfiguration configuration = new EnvironmentConfiguration();
            await configuration.ConfigureTestDatabase();
        }

        [Fact]
        public async Task CreateNewProductSuccessfully() {
            //Arrange
            var productClient = new RestClient("http://localhost:5212/api/Product");
            var producerClient = new RestClient("http://localhost:5212/api/Producer");

            var productRequest = new RestRequest();
            var producerRequest = new RestRequest();

            byte[] picture = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            var productDTO = new CreateProductDTO(
                "Product",
                "Description",
                picture,
                Category.VEGETABLE,
                10.10,
                Unit.UNIT,
                10,
                true,
                "22/11/2023",
                new Guid()
            );

            var producerDto = new CreateProducerDTO {
                Name = "Producer Test",
                Email = "test@test.com",
                AttendedCities = "City1;City2;City3",
                CreatedAt = DateTime.Now,
                FavdByConsumers = new List<ConsumerFavProducer>(),
                CPF = "111.111.111-11",
                OriginCity = "City1",
                Password = "123",
                Telephone = "(31) 99999-9999",
                WhereToFind = "Local de encontro"
            };

            //Act
            producerRequest.Method = Method.Post;
            producerRequest.AddJsonBody(JsonConvert.SerializeObject(producerDto));
            producerRequest.AddHeader("Content-Type", "application/json");

            RestResponse producerResponse = await producerClient.ExecuteAsync(producerRequest);

            var responseObj = JsonConvert.DeserializeObject<ListProducerDTO>(producerResponse.Content);

            productDTO.ProducerId = responseObj?.Id;

            productRequest.Method = Method.Post;
            productRequest.AddJsonBody(JsonConvert.SerializeObject(productDTO));
            productRequest.AddHeader("Content-Type", "application/json");

            RestResponse productResponse = await productClient.ExecuteAsync(productRequest);
            var producerResponseObj = JsonConvert.DeserializeObject<ListProductDTO>(productResponse.Content);


            Assert.True(productResponse.IsSuccessful);

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

