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
using backend.Product.Exceptions;
using Tests.Shared.Utils;

namespace Tests.IntegrationTests.v1
{
    public class ProductControllerTest:IAsyncLifetime, IClassFixture<EnvironmentConfiguration> {

        private EnvironmentConfiguration _environmentConfiguration;
        private const string _baseApiUrl = "http://localhost:5212/api";

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
        public async Task Save_GivenProductAndNotExistentProducer_ThrowsException() {
            //Arrange
            var productClient = new RestClient("http://localhost:5212/api/Product");

            var productRequest = new RestRequest();

            var productDTO = new List<CreateProductDTO>() {
                new ProductDTOFactory()
                    .WithProducerId(Guid.NewGuid())
                    .Build()
            };
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
        public async Task Save_GivenMissingBody_ThrowsError() {
            
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

        [Fact]
        public async Task GetProducerProducts_GivenProducerAndProduct_ReturnsListProductsDTO() {
            //Arrange
            var productClient = new RestClient($"{_baseApiUrl}/Product");
            var producerClient = new RestClient($"{_baseApiUrl}/Producer");

            var productRequest = new RestRequest();
            var producerRequest = new RestRequest();

            var producerDTO = new ProducerDTOFactory().Build();

            //Act
            producerRequest.Method = Method.Post;
            producerRequest.AddJsonBody(JsonConvert.SerializeObject(producerDTO));
            producerRequest.AddHeader("Content-Type", "application/json");

            RestResponse producerResponse = await producerClient.ExecuteAsync(producerRequest);

            var producerResponseObj = JsonConvert.DeserializeObject<ListProducerDTO>(producerResponse.Content);

            List<CreateProductDTO> productsDTO = new List<CreateProductDTO>();

            for (int i = 0; i < 11; i++) {
                //Adding 11 products to generate 2 pages of pagination
                productsDTO.Add(new ProductDTOFactory()
                    .WithName($"aa{i.ToString()}")
                    .WithProducerId(producerResponseObj.Id)
                    .Build()
                );
            }

            productRequest.Method = Method.Post;
            productRequest.AddJsonBody(JsonConvert.SerializeObject(productsDTO));
            productRequest.AddHeader("Content-Type", "multipart/form-data");
            productRequest.AddFile("0", GetPictureBuilderUtils.BananaPath,"image/png");
            productRequest.AddFile("1", GetPictureBuilderUtils.StrawberryPath, "image/png");

            RestResponse productResponse = await productClient.ExecuteAsync(productRequest);
            var productResponseObj = JsonConvert.DeserializeObject<List<ListProductDTO>>(productResponse.Content);

            //Assert
            Assert.True(productResponse.IsSuccessful);
            Assert.True(producerResponse.IsSuccessful);

            producerResponseObj.Should().BeEquivalentTo(producerDTO, options => options.ExcludingMissingMembers());

            for (int i = 0; i < 2; i++) {
                productResponseObj!
                    .ElementAt(i)
                    .Should()
                    .BeEquivalentTo(
                        productsDTO.ElementAt(i), options => options
                            .ExcludingMissingMembers()
                            .Excluding(dto => dto.HarvestDate)
                    );
                Assert.Equal(productResponseObj!.ElementAt(i).HarvestDate, DateUtils.ConvertStringToDateTime(productsDTO.ElementAt(i).HarvestDate!, "dd/MM/yyyy"));
            }
        }


        public async Task GetProducerProducts_GivenProducerId_ReturnsListProductsResponse() {
            
            //Arrange
            var producerClient = new RestClient("http://localhost:5212/api/Producer");
            var productClient = new RestClient("http://localhost:5212/api/Product");

            var producerRequest = new RestRequest();
            var productRequest = new RestRequest();

            var resultsPerPage = 10;

            productRequest.Method = Method.Post;
        //    productRequest.AddJsonBody(JsonConvert.SerializeObject(productDTO));
            productRequest.AddHeader("Content-Type", "application/json");
        }

    }
}

