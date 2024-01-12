using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Product.Enums;

namespace Tests.Shared.Factories.Order {
    public class OrderFactory {
        private backend.Models.Order order = new() {
            Id = Guid.NewGuid(),
            ConsumerId = Guid.NewGuid(),
            ProducerId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            Status = Status.PENDING,
        };

        public backend.Models.Order Build() {
            return order;
        }

        public OrderFactory WithId(Guid id) {
            order.Id = id;
            return this;
        }

        public OrderFactory WithConsumerId(Guid consumerId) {
            order.ConsumerId = consumerId;
            return this;
        }

        public OrderFactory WithProducerId(Guid producerId) {
            order.ProducerId = producerId;
            return this;
        }

        public OrderFactory WithQuantity(int quantity) {
            order.Quantity = quantity;
            return this;
        }

        public OrderFactory WithStatus(Status status) { 
            order.Status = status;
            return this;
        }

        public OrderFactory WithCreatedAt(DateTime createdAt) {
            order.CreatedAt = createdAt;
            return this;
        }
    }
}
