using backend.Order.DTOs;
using backend.Product.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Order.Repository {
    public interface IOrderRepository {

        Task<Models.Order> Create(Models.Order order);

        Task<Models.Order> Delete(Models.Order order);
        Task<Models.Order> FindById(Guid orderId);
        ListOrdersPagination GetOrdersFromProducer(Guid producerId, int page, int pageResults);
        ListOrdersPagination GetOrdersFromConsumer(Guid consumerId, int page, int pageResults);
        Task<Models.Order> UpdateStatus(Status status);
    }
}
