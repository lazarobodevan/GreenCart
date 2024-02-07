using backend.Contexts;
using backend.Order.Exceptions;
using backend.Order.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Shared.Factories.Order;

namespace UnitTests.UnitTests.Repositories {
    public class OrderRepositoryTest : IAsyncLifetime{
        private DbContextOptions<DatabaseContext> _dbContextOptions;
        DatabaseContext _databaseContext;

        public OrderRepositoryTest() {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "OrderRepositoryTest").Options;
            _databaseContext = new DatabaseContext(_dbContextOptions);
        }

        public Task DisposeAsync() {
            //Reset database
            return _databaseContext.Database.EnsureDeletedAsync();

        }

        public Task InitializeAsync() {
            //throw new NotImplementedException();
            return _databaseContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task Create_GivenOrderEntity_ReturnsCreatedOrderEntity() {
            //Arrange
            OrderRepository orderRepository = new OrderRepository(_databaseContext);
            var order = new OrderFactory().Build();

            //Act
            var createdOrder = await orderRepository.Create(order);

            //Assert
            Assert.NotNull(createdOrder);
        }

        [Fact]
        public async Task Delete_GivenOrderId_ReturnsDeletedOrder() {
            
            //Arrange
            OrderRepository orderRepository = new OrderRepository(_databaseContext);
            var order = new OrderFactory().Build();

            //Act
            var createdOrder = await orderRepository.Create(order);
            _databaseContext.ChangeTracker.Clear();
            var deletedOrder = await orderRepository.Delete(new backend.Models.Order(createdOrder));

            //Assert
            Assert.NotNull(deletedOrder);
            Assert.NotEqual(createdOrder.DeletedAt, deletedOrder.DeletedAt);
        }

        [Fact]
        public async Task GetOrdersFromProducer_GivenProducerId_ReturnsPaginatedOrders() {
            //Arrange
            var producerId = Guid.NewGuid();

            OrderRepository orderRepository = new OrderRepository(_databaseContext);

            //Producer 1 has 2 orders
            var order= new OrderFactory()
                .WithProducerId(producerId)
                .Build();

            //Act
            var createdOrder = await orderRepository.Create(order);
            _databaseContext.ChangeTracker.Clear();
            var foundOrders = orderRepository.GetOrdersFromProducer(producerId, 0,2);

            //Assert
            Assert.NotNull(foundOrders);
            Assert.Single(foundOrders.Data);
            Assert.Equal(1, foundOrders.Pages);
            Assert.Equal(0, foundOrders.CurrentPage);
        }

        [Fact]
        public async Task GetOrdersFromConsumer_GivenConsumerId_ReturnsPaginatedOrders() {
            //Arrange
            var consumerId = Guid.NewGuid();

            OrderRepository orderRepository = new OrderRepository(_databaseContext);

            //Producer 1 has 2 orders
            var order = new OrderFactory()
                .WithConsumerId(consumerId)
                .Build();

            //Act
            var createdOrder = await orderRepository.Create(order);
            _databaseContext.ChangeTracker.Clear();
            var foundOrders = orderRepository.GetOrdersFromConsumer(consumerId, 0, 2);

            //Assert
            Assert.NotNull(foundOrders);
            Assert.Single(foundOrders.Data);
            Assert.Equal(1, foundOrders.Pages);
            Assert.Equal(0, foundOrders.CurrentPage);
        }
    }
}
