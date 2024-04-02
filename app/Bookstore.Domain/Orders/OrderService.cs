﻿using Amazon.CloudWatch.EMF.Logger;
using Amazon.CloudWatch.EMF.Model;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Customers;
using Microsoft.Extensions.Logging;

namespace Bookstore.Domain.Orders
{
    public interface IOrderService
    {
        Task<IPaginatedList<Order>> GetOrdersAsync(OrderFilters filters, int pageIndex = 1, int pageSize = 10);

        Task<IEnumerable<Order>> GetOrdersAsync(string sub);

        Task<Order> GetOrderAsync(int id);

        Task<OrderStatistics> GetStatisticsAsync();

        Task<int> CreateOrderAsync(CreateOrderDto createOrderDto);

        Task UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto);

        Task CancelOrderAsync(CancelOrderDto cancelOrderDto);

    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<OrderService> logger;
        private readonly IMetricsLogger metricsLogger;

        public OrderService(IOrderRepository orderRepository,
            IShoppingCartRepository shoppingCartRepository,
            ICustomerRepository customerRepository,
            ILoggerFactory logger,
            IMetricsLogger metricsLogger)
        {
            this.orderRepository = orderRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this.customerRepository = customerRepository;
            this.logger = logger.CreateLogger<OrderService>();
            this.metricsLogger = metricsLogger; 
        }

        public async Task<IPaginatedList<Order>> GetOrdersAsync(OrderFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            return await orderRepository.ListAsync(filters, pageIndex, pageSize);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string sub)
        {
            return await orderRepository.ListAsync(sub);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await orderRepository.GetAsync(id);
        }

        public async Task<OrderStatistics> GetStatisticsAsync()
        {
            return (await orderRepository.GetStatisticsAsync()) ?? new OrderStatistics();
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var shoppingCart = await shoppingCartRepository.GetAsync(dto.CorrelationId);

            var customer = await customerRepository.GetAsync(dto.CustomerSub);

            var order = new Order(customer.Id, dto.AddressId);

            await orderRepository.AddAsync(order);

            shoppingCart.GetShoppingCartItems(ShoppingCartItemFilter.ExcludeOutOfStockItems).ToList().ForEach(x =>
            {
                order.AddOrderItem(x.Book, x.Quantity);

                x.Book.ReduceStockLevel(x.Quantity);

                shoppingCart.RemoveShoppingCartItemById(x.Id);
            });

            // Because each repository implements a unit of work, changes to the shopping cart and to stock levels 
            // are captured by the unit of work and can be persisted by called SaveChangesAsync on _any_ repository.
            await orderRepository.SaveChangesAsync();

            watch.Stop();
            EmitOrderMetrics(order, watch.ElapsedMilliseconds);
            logger.LogInformation("Created a new OrderId {orderid} for the CustomerId {customerid}", customer.Id, order.Id);

            return order.Id;
        }

        public async Task UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await orderRepository.GetAsync(dto.OrderId);

            order.OrderStatus = dto.OrderStatus;

            order.UpdatedOn = DateTime.UtcNow;

            await orderRepository.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(CancelOrderDto dto)
        {
            var order = await orderRepository.GetAsync(dto.OrderId, dto.CustomerSub);

            if (order == null) return;

            order.OrderStatus = OrderStatus.Cancelled;

            await orderRepository.SaveChangesAsync();
        }

        private void EmitOrderMetrics(Order order, long processingTimeMilliseconds)
        {
            //Add Dimensions
            var dimensionSet = new DimensionSet();
            dimensionSet.AddDimension("Application", "BobsUsedBookstore");

            metricsLogger.SetDimensions(dimensionSet);

            metricsLogger.PutMetric("NumberOfOrders", 1, Unit.COUNT);
            metricsLogger.PutMetric("BooksSold", order.OrderItems.Count(), Unit.COUNT);
            metricsLogger.PutMetric("SaleAmount", Decimal.ToDouble(order.OrderItems.Sum(x => x.Quantity * x.Book.Price)), Unit.COUNT);
            metricsLogger.PutMetric("ProcessingTime", processingTimeMilliseconds, Unit.MILLISECONDS);

            metricsLogger.PutProperty("CustomerID", order.CustomerId);

            metricsLogger.Flush();
        }
    }
}