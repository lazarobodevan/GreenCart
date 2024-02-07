using backend.Contexts;
using backend.Models;
using backend.Order.DTOs;
using backend.Order.Exceptions;
using backend.Product.Enums;
using backend.Product.Exceptions;
using backend.Shared.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Order.Repository {
    public class OrderRepository : IOrderRepository {

        private readonly DatabaseContext _dbContext;

        public OrderRepository(DatabaseContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Models.Order> Create(Models.Order order) {
            try {
                order.CreatedAt = DateTime.UtcNow;
                var createdOrder = await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();

                return createdOrder.Entity;
            }catch(Exception ex) {
                throw new Exception($"Erro ao criar encomenda: {ex.Message}");
            }
        }

        public async Task<Models.Order> Delete(Models.Order order) {
            try {
                order.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                return order;
            } catch(Exception ex) {
                throw new Exception($"Falha ao deletar encomenda: {ex.Message}");
            }

        }

        public Task<Models.Order> FindById(Guid orderId) {
            throw new NotImplementedException();
        }

        public Pagination<Models.Order> GetOrdersFromConsumer(Guid consumerId, int page, int pageResults) {
            var ordersExists = _dbContext.Orders.Any<Models.Order>(order => order.ConsumerId == consumerId && order.DeletedAt == null);

            var ordersQuery = _dbContext.Orders.Where(order =>
                order.ConsumerId == consumerId &&
                order.DeletedAt == null
            ).OrderBy(order => order.CreatedAt)
            .ToList();

            var totalOrdersCount = ordersQuery.Count();
            var pageCount = (int)Math.Ceiling((double)totalOrdersCount / pageResults);

            page = Math.Min(page, (int)pageCount - 1);

            int offset = Math.Max(0, page) * pageResults;

            var orders = ordersQuery
                .Skip(offset)
                .Take((int)pageResults)
                .ToList();

            return new Pagination<backend.Models.Order>() {
                Pages = totalOrdersCount,
                CurrentPage = page,
                Data = orders,

            };
        }

        public Pagination<Models.Order> GetOrdersFromProducer(Guid producerId, int page, int pageResults) {
            var ordersExists = _dbContext.Orders.Any<Models.Order>(order => order.ProducerId == producerId && order.DeletedAt == null);

            var ordersQuery = _dbContext.Orders.Where(order =>
                order.ProducerId == producerId &&
                order.DeletedAt == null
            ).OrderBy(order => order.CreatedAt)
            .ToList();

            var totalOrdersCount = ordersQuery.Count();
            var pageCount = (int)Math.Ceiling((double)totalOrdersCount / pageResults);

            page = Math.Min(page, (int)pageCount-1);

            int offset = Math.Max(0, page) * pageResults;

            var orders = ordersQuery
                .Skip(offset)
                .Take((int)pageResults)
                .ToList();

            return new Pagination<Models.Order>() {
                Pages = pageCount,
                CurrentPage = page,
                Data = orders,

            };
        }

        public Task<Models.Order> UpdateStatus(Status status) {
            throw new NotImplementedException();
        }
    }
}
